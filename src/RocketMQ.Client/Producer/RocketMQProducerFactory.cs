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

using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using RocketMQ.Remoting;

namespace RocketMQ.Client.Producer
{
    public class RocketMQProducerFactory : IRocketMQProducerFactory
    {
        private readonly ConcurrentDictionary<string, IRocketMQProducer> _producers = new();
        private readonly IOptionsFactory<RocketMQProducerOptions> _optionsFactory;
        private readonly IRemotingClient _remotingClient;

        public RocketMQProducerFactory(
            IOptionsFactory<RocketMQProducerOptions> optionsFactory,
            IRemotingClient remotingClient)
        {
            _optionsFactory = optionsFactory;
            _remotingClient = remotingClient;
        }

        public IRocketMQProducer CreateProducer(string name)
        {
            return _producers.GetOrAdd(name, _ =>
            {
                var options = _optionsFactory.Create(name);
                return new RocketMQProducer(options, _remotingClient);
            });
        }

        public void Dispose()
        {
            foreach (var producer in _producers.Values)
            {
                producer.Dispose();
            }
        }
    }
}