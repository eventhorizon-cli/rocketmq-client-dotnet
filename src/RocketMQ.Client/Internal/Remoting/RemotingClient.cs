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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Bedrock.Framework;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Logging;
using RocketMQ.Client.Internal.Remoting.Protocol;

namespace RocketMQ.Client.Internal.Remoting
{
    internal class RemotingClient : IRemotingClient
    {
        private readonly ILogger _logger;

        private readonly IConnectionFactory _connectionFactory;
        private readonly Dictionary<EndPoint, MQConnection> _connections;
        private readonly ConcurrentDictionary<int, TaskCompletionSource<RemotingCommand>> _responseCallbacks;
        private readonly SemaphoreSlim _semaphoreSlim;
        private readonly IRemoteCommandSerializer _serializer;

        public RemotingClient(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<RemotingClient>();

            _connectionFactory = new ClientBuilder().UseSockets().Build();
            _connections = new Dictionary<EndPoint, MQConnection>();
            _responseCallbacks = new ConcurrentDictionary<int, TaskCompletionSource<RemotingCommand>>();
            _semaphoreSlim = new SemaphoreSlim(1);
            _serializer = new RemoteCommandSerializer();
        }

        public async Task<RemotingCommand> InvokeAsync(
            EndPoint endPoint,
            RemotingCommand request,
            TimeSpan timeout)
        {
            using var cts = new CancellationTokenSource(timeout);
            var cancellationToken = cts.Token;
            cancellationToken.Register(() =>
            {
                if (_responseCallbacks.TryRemove(request.Opaque, out var tcs))
                {
                    tcs.TrySetCanceled(cancellationToken);
                }
            });

            var tcs = new TaskCompletionSource<RemotingCommand>();
            _responseCallbacks.TryAdd(request.Opaque, tcs);

            try
            {
                var connection = await ConnectAsync(endPoint, cancellationToken);
                await connection.SendAsync(request, cancellationToken);
                return await tcs.Task;
            }
            catch
            {
                _responseCallbacks.TryRemove(request.Opaque, out _);
                throw;
            }
        }

        public async Task InvokeOneway(
            EndPoint endPoint,
            RemotingCommand request,
            TimeSpan timeout)
        {
            using var cts = new CancellationTokenSource(timeout);
            var cancellationToken = cts.Token;
            var connection = await ConnectAsync(endPoint, cancellationToken);
            await connection.SendAsync(request, cancellationToken);
        }

        public async ValueTask DisposeAsync()
        {
            foreach (var tcs in _responseCallbacks.Values)
            {
                tcs.SetException(new ObjectDisposedException(nameof(MQConnection)));
            }

            await _semaphoreSlim.WaitAsync();
            try
            {
                foreach (var connection in _connections.Values)
                {
                    await connection.DisposeAsync();
                }
            }
            finally
            {
                _semaphoreSlim.Release();
            }

            _semaphoreSlim.Dispose();
        }

        private async ValueTask<MQConnection> ConnectAsync(EndPoint endPoint, CancellationToken cancellationToken)
        {
            try
            {
                await _semaphoreSlim.WaitAsync(cancellationToken);
                if (_connections.TryGetValue(endPoint, out var rocketMqConnection))
                {
                    return rocketMqConnection;
                }

                var connectionContext = await _connectionFactory.ConnectAsync(endPoint, cancellationToken);
                connectionContext.ConnectionClosed.Register(() => RemoveConnection(endPoint));
                rocketMqConnection = new MQConnection(connectionContext, _serializer);
                _ = ReceiveAsync(rocketMqConnection);
                _connections[endPoint] = rocketMqConnection;
                return rocketMqConnection;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        private async Task RemoveConnection(EndPoint endPoint)
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                _connections.Remove(endPoint);
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        private async Task ReceiveAsync(MQConnection connection)
        {
            while (true)
            {
                try
                {
                    var response = await connection.ReceiveAsync();

                    _logger.LogDebug($"Received response {response.Opaque}");

                    if (_responseCallbacks.TryRemove(response.Opaque, out var tcs))
                    {
                        tcs.SetResult(response);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Failed to receive response");
                    foreach (var tcs in _responseCallbacks.Values)
                    {
                        tcs.SetException(e);
                    }

                    break;
                }
            }
        }
    }
}