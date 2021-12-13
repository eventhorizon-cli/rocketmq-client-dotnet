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

using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;

namespace RocketMQ.Client.Internal.Remoting.Protocol
{
    internal class RemotingCommand
    {
        private const int ResponseType = 1;

        private static int _opaque = 0;

        public short Code { get; set; }

        public LanguageCode LanguageCode { get; set; } 

        public int Version { get; set; }

        public int Opaque { get; set; } 

        public int Flag { get; set; }

        public string Remark { get; set; }

        public Dictionary<string, object> ExtFields { get; set; } 

        [JsonIgnore] public byte[]? Body { get; set; } 

        public RemotingCommand()
        {
        }

        public RemotingCommand(short code, CommandCustomHeader header)
        {
            LanguageCode =  LanguageCode.Dotnet;
            Opaque = Interlocked.Increment(ref _opaque);
            Code = code;
            Version = 317; // TODO
            ExtFields = header.Encode();
        }

        public bool IsResponseType() => (Flag & ResponseType) == ResponseType;
    }
}