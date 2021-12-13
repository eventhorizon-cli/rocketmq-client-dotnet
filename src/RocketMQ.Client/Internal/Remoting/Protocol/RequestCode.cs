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

namespace RocketMQ.Client.Internal.Remoting.Protocol
{
    public static class RequestCode
    {
        public const short SendMessage = 10;
        public const short PullMessage = 11;
        public const short QueryMessage = 12;
        public const short QueryConsumerOffset = 14;
        public const short UpdateConsumerOffset = 15;
        public const short CreateTopic = 17;
        public const short SearchOffsetByTimestamp = 29;
        public const short GetMaxOffset = 30;
        public const short GetMinOffset = 31;
        public const short ViewMessageByID = 33;
        public const short HeartBeat = 34;
        public const short ConsumerSendMsgBack = 36;
        public const short ENDTransaction = 37;
        public const short GetConsumerListByGroup = 38;
        public const short LockBatchMQ = 41;
        public const short UnlockBatchMQ = 42;
        public const short GetRouteInfoByTopic = 105;
        public const short GetBrokerClusterInfo = 106;
        public const short SendBatchMessage = 320;
        public const short CheckTransactionState = 39;
        public const short NotifyConsumerIdsChanged = 40;
        public const short GetAllTopicListFromNameServer = 206;
        public const short DeleteTopicInBroker = 215;
        public const short DeleteTopicInNameSrv = 216;
        public const short ResetConsumerOffset = 220;
        public const short GetConsumerRunningInfo = 307;
        public const short ConsumeMessageDirectly = 309;
    }
}