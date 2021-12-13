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

using System.Threading;
using System.Threading.Tasks;
using RocketMQ.Client.Internal;

namespace RocketMQ.Client.Producer
{
    internal class MQProducer : IMQProducer
    {
        private readonly MQProducerOptions _options;
        private readonly IMQClient _mqClient;

        public MQProducer(MQProducerOptions options,IMQClient mqClient)
        {
            _options = options;
            _mqClient = mqClient;
        }

        public void Start()
        {
            throw new System.NotImplementedException();
        }

        public void Shutdown()
        {
            throw new System.NotImplementedException();
        }

        public Task<SendResult> SendAsync(Message message, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task SendOnewayAsync(Message message, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
    }
}