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

namespace RocketMQ.Client.Producer
{
    public class MQProducerOptions : MQClientOptions
    {
        /// <summary>
        /// Producer group conceptually aggregates all producer instances of exactly same role, which is particularly
        /// important when transactional messages are involved.
        /// </summary>
        /// <remarks>
        /// <para>For non-transactional messages, it does not matter as long as it's unique per process.</para>
        /// </remarks>
        /// <footer>See <a href="http://rocketmq.apache.org/docs/core-concept/"></a> for more discussion.</footer>
        public string GroupName { get; set; }

        /// <summary>
        /// Number of queues to create per default topic.
        /// </summary>
        public int DefaultTopicQueueNums { get; set; } = 4;

        /// <summary>
        /// Timeout for sending messages.
        /// </summary>
        public TimeSpan SendMsgTimeout { get; set; } = TimeSpan.FromSeconds(3);

        /// <summary>
        /// Compress message body threshold, namely, message body larger than 4k will be compressed on default.
        /// </summary>
        public int CompressMsgBodyOverHowmuch { get; set; } = 1024 * 4;

        /// <summary>
        /// Maximum number of retry to perform internally before claiming sending failure in synchronous mode.
        /// </summary>
        /// <remarks>
        /// This may potentially cause message duplication which is up to application developers to resolve.
        /// </remarks>
        public int RetryTimesWhenSendFailed { get; set; } = 2;

        /// <summary>
        /// Indicate whether to retry another broker on sending failure internally.
        /// </summary>
        public bool RetryTimesWhenSendAsyncFailed { get; set; } = false;
        
        /// <summary>
        /// Maximum allowed message size in bytes.
        /// </summary>
        public int MaxMessageSize { get; set; } = 1024 * 1024 * 4; // 4M
    }
}