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
    internal static class ResponseCodes
    {
        public const short ResSuccess = 0;
        public const short ResError = 1;
        public const short ResFlushDiskTimeout = 10;
        public const short ResSlaveNotAvailable = 11;
        public const short ResFlushSlaveTimeout = 12;
        public const short ResTopicNotExist = 17;
        public const short ResPullNotFound = 19;
        public const short ResPullRetryImmediately = 20;
        public const short ResPullOffsetMoved = 21;
    }
}