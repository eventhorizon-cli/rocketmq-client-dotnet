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

namespace RocketMQ.Client.Producer
{
    public class SendResult
    {
        public SendStatus SendStatus { get; }
        public string MsgId { get; }
        public MessageQueue MessageQueue { get; }
        public long QueueOffset { get; }
        public string TransactionId { get; }
        public string OffsetMsgId { get; }
        public string RegionId { get; }
        public bool TraceOn { get; } = true;


        public SendResult(SendStatus sendStatus, string msgId, string offsetMsgId, MessageQueue messageQueue, long queueOffset)
        {
            SendStatus = sendStatus;
            MsgId = msgId;
            OffsetMsgId = offsetMsgId;
            MessageQueue = messageQueue;
            QueueOffset = queueOffset;
        }

        public SendResult(SendStatus sendStatus, string msgId, MessageQueue messageQueue, long queueOffset, string transactionId, string offsetMsgId, string regionId)
        {
            SendStatus = sendStatus;
            MsgId = msgId;
            MessageQueue = messageQueue;
            QueueOffset = queueOffset;
            TransactionId = transactionId;
            OffsetMsgId = offsetMsgId;
            RegionId = regionId;
        }
    }
}