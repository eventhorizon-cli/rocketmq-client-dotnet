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
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RocketMQ.Client.Infrastructure.Internal;
using RocketMQ.Client.Internal.Remoting;
using RocketMQ.Client.Internal.Remoting.Protocol;

namespace RocketMQ.Client.Internal.Route
{
    internal class NameServer : INameServer
    {
        private readonly IRemotingClient _remotingClient;

        public NameServer(IRemotingClient remotingClient)
        {
            _remotingClient = remotingClient;
        }

        public async Task<TopicRouteData> GetTopicRouteInfoAsync(
            string topic,
            TimeSpan timeout,
            bool allTopicNotExist = true,
            HashSet<int> logicalQueueIdsFilter = null)
        {
            var requestHeader = new GetRouteInfoRequestHeader
            {
                Topic = topic,
                SysFlag = MessageSysFlag.LogicalQueueFlag,
                LogicalQueueIdsFilter = logicalQueueIdsFilter
            };

            var request = new RemotingCommand(RequestCode.GetRouteInfoByTopic, requestHeader);
            var endPoint = IPEndPoint.Parse("192.168.1.11:9876");
            var response = await _remotingClient.InvokeAsync(endPoint, request, timeout);
            var data = Encoding.UTF8.GetString(response.Body).FromJson<TopicRouteData>();
            return data;
        }
    }
}