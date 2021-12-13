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

namespace RocketMQ.Client.Internal.Route
{
    public class QueueData : IEquatable<QueueData>
    {
        public string BrokerName { get; set; }

        public int ReadQueueNums { get; set; }

        public int WriteQueueNums { get; set; }

        public int Perm { get; set; }

        public int TopicSynFlag { get; set; }

        public bool Equals(QueueData? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return BrokerName == other.BrokerName
                   && ReadQueueNums == other.ReadQueueNums
                   && WriteQueueNums == other.WriteQueueNums
                   && Perm == other.Perm
                   && TopicSynFlag == other.TopicSynFlag;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((QueueData)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BrokerName, ReadQueueNums, WriteQueueNums, Perm, TopicSynFlag);
        }
    }
}