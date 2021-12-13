/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Text;
using RocketMQ.Client.Infrastructure.Internal;

namespace RocketMQ.Client.Internal.Remoting.Protocol
{
    // Frame format:
    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // + item | frame size | header length |     header body     |     body     +
    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // + len  |   4bytes   |     4bytes    | header length bytes | remain bytes +
    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    internal class RemoteCommandSerializer : IRemoteCommandSerializer
    {
        private readonly SerializeType _serializeType = SerializeType.Json;

        public bool TryParseMessage(
            in ReadOnlySequence<byte> input,
            ref SequencePosition consumed,
            ref SequencePosition examined,
            out RemotingCommand command)
        {
            command = null!;
            if (input.Length < 4)
            {
                return false;
            }

            int index = 0;
            // frame size
            Span<byte> frameSizeBuffer = stackalloc byte[4];
            input.Slice(index, 4).CopyTo(frameSizeBuffer);
            int frameSize = BinaryPrimitives.ReadInt32BigEndian(frameSizeBuffer);
            if (input.Length < 4 + frameSize)
            {
                return false;
            }

            index += 4;

            // header length
            Span<byte> headerLengthBuffer = stackalloc byte[4];
            input.Slice(index, 4).CopyTo(headerLengthBuffer);
            int oriHeaderLength = BinaryPrimitives.ReadInt32BigEndian(headerLengthBuffer);
            int headerLength = oriHeaderLength & 0xFFFFFF;
            index += 4;

            // serialize type
            var serializeType = (SerializeType)((oriHeaderLength >> 24) & 0xFF);

            // header body
            var headerData = input.Slice(index, headerLength).FirstSpan;
            command = serializeType switch
            {
                SerializeType.Json => JsonHeaderDecode(headerData),
                SerializeType.RocketMQ => RocketMQHeaderDecode(headerData),
                _ => throw new ArgumentOutOfRangeException()
            };
            index += headerLength;

            // body
            var bodyData = input.Slice(index).FirstSpan.ToArray();
            command.Body = bodyData;
            return true;

            static RemotingCommand JsonHeaderDecode(ReadOnlySpan<byte> headerData)
            {
                var headerJson = Encoding.UTF8.GetString(headerData);
                var header = headerJson.FromJson<RemotingCommand>();
                return header!;
            }

            // TODO
            static RemotingCommand RocketMQHeaderDecode(ReadOnlySpan<byte> headerData)
            {
                throw new NotImplementedException();
            }
        }

        public void WriteMessage(RemotingCommand command, IBufferWriter<byte> output)
        {
            var headerData = _serializeType switch
            {
                SerializeType.Json => JsonHeaderEncode(command),
                SerializeType.RocketMQ => RocketMQHeaderEncode(command),
                _ => throw new ArgumentOutOfRangeException()
            };

            int frameSize = 4 + headerData.Length;
            if (command.Body != null)
            {
                frameSize += command.Body.Length;
            }

            // frame size itself is 4 bytes
            var buffer = output.GetSpan(4 + frameSize);

            int index = 0;
            // frame size
            BinaryPrimitives.WriteInt32BigEndian(buffer, frameSize);
            index += 4;
            // header length
            MarkProtocolType(headerData.Length, _serializeType).CopyTo(buffer[index..]);
            index += 4;
            // header body
            headerData.CopyTo(buffer[index..]);
            index += headerData.Length;
            // body
            command.Body?.CopyTo(buffer[index..]);

            output.Advance(4 + frameSize);

            static byte[] MarkProtocolType(int source, SerializeType serializeType)
            {
                byte[] result = new byte[4];

                result[0] = (byte)serializeType;
                result[1] = (byte)((source >> 16) & 0xFF);
                result[2] = (byte)((source >> 8) & 0xFF);
                result[3] = (byte)(source & 0xFF);
                return result;
            }

            static byte[] JsonHeaderEncode(RemotingCommand command) =>
                Encoding.UTF8.GetBytes(command.ToJson());

            // TODO
            static byte[] RocketMQHeaderEncode(RemotingCommand command)
            {
                throw new NotImplementedException();
            }
        }
    }
}