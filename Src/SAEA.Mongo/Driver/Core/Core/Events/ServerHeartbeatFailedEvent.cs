/* Copyright 2013-present MongoDB Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using SAEA.Mongo.Driver.Core.Clusters;
using SAEA.Mongo.Driver.Core.Connections;
using SAEA.Mongo.Driver.Core.Servers;

namespace SAEA.Mongo.Driver.Core.Events
{
    /// <preliminary/>
    /// <summary>
    /// Occurs when a heartbeat failed.
    /// </summary>
    public struct ServerHeartbeatFailedEvent
    {
        private readonly ConnectionId _connectionId;
        private readonly Exception _exception;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerHeartbeatFailedEvent"/> struct.
        /// </summary>
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="exception">The exception.</param>
        public ServerHeartbeatFailedEvent(ConnectionId connectionId, Exception exception)
        {
            _connectionId = connectionId;
            _exception = exception;
        }

        /// <summary>
        /// Gets the cluster identifier.
        /// </summary>
        public ClusterId ClusterId
        {
            get { return _connectionId.ServerId.ClusterId; }
        }

        /// <summary>
        /// Gets the connection identifier.
        /// </summary>
        public ConnectionId ConnectionId
        {
            get { return _connectionId; }
        }

        /// <summary>
        /// Gets the exception.
        /// </summary>
        public Exception Exception
        {
            get { return _exception; }
        }

        /// <summary>
        /// Gets the server identifier.
        /// </summary>
        public ServerId ServerId
        {
            get { return _connectionId.ServerId; }
        }
    }
}
