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
using System.Text;

namespace RocketMQ.Client
{
    public abstract class MQClientOptions
    {
        public string NamesrvAddr { get; set; }

        public string ClientIP { get; set; }

        public string InstanceName { get; set; }

        public string Namespace { get; set; }

        /// <summary>
        /// Pulling topic information interval from the named server.
        /// </summary>
        public TimeSpan PollNameServerInterval { get; set; } = TimeSpan.FromSeconds(30);

        /// <summary>
        /// Heartbeat interval in microseconds with message broker.
        /// </summary>
        public TimeSpan HeartbeatBrokerInterval { get; set; } = TimeSpan.FromSeconds(30);

        /// <summary>
        /// Offset persistent interval for consumer.
        /// </summary>
        public TimeSpan PersistConsumerOffsetInterval { get; set; } = TimeSpan.FromSeconds(5);

        public TimeSpan PullTimeDelayMillsWhenException { get; set; } = TimeSpan.FromSeconds(1);

        /// <summary>
        /// TODO: What is the meaning of this property?
        /// </summary>
        public bool UnitMode { get; set; } = false;

        public string UnitName { get; set; }

        public bool VipChannelEnabled { get; set; } = false;

        public bool UseTLS { get; set; } = false;

        internal string BuildMQClientId()
        {
            var sb = new StringBuilder();
            sb.Append(ClientIP);
            sb.Append("@");
            sb.Append(InstanceName);
            if (string.IsNullOrWhiteSpace(UnitName))
            {
                sb.Append("@");
                sb.Append(UnitName);
            }

            return sb.ToString();
        }
    }
}