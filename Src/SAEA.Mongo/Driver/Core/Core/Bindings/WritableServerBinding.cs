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
using System.Threading;
using System.Threading.Tasks;
using SAEA.Mongo.Driver.Core.Clusters;
using SAEA.Mongo.Driver.Core.Clusters.ServerSelectors;
using SAEA.Mongo.Driver.Core.Misc;
using SAEA.Mongo.Driver.Core.Servers;

namespace SAEA.Mongo.Driver.Core.Bindings
{
    /// <summary>
    /// Represents a write binding to a writable server.
    /// </summary>
    public sealed class WritableServerBinding : IReadWriteBinding
    {
        // fields
        private readonly ICluster _cluster;
        private bool _disposed;
        private readonly ICoreSessionHandle _session;

        // constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="WritableServerBinding" /> class.
        /// </summary>
        /// <param name="cluster">The cluster.</param>
        /// <param name="session">The session.</param>
        public WritableServerBinding(ICluster cluster, ICoreSessionHandle session)
        {
            _cluster = Ensure.IsNotNull(cluster, nameof(cluster));
            _session = Ensure.IsNotNull(session, nameof(session));
        }

        // properties
        /// <inheritdoc/>
        public ReadPreference ReadPreference
        {
            get { return ReadPreference.Primary; }
        }

        /// <inheritdoc/>
        public ICoreSessionHandle Session
        {
            get { return _session; }
        }

        // methods
        /// <inheritdoc/>
        public IChannelSourceHandle GetReadChannelSource(CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            var server = _cluster.SelectServerAndPinIfNeeded(_session, WritableServerSelector.Instance, cancellationToken);
            
            return GetChannelSourceHelper(server);
        }

        /// <inheritdoc/>
        public async Task<IChannelSourceHandle> GetReadChannelSourceAsync(CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            var server = await _cluster.SelectServerAndPinIfNeededAsync(_session, WritableServerSelector.Instance, cancellationToken).ConfigureAwait(false);
            return GetChannelSourceHelper(server);
        }

        /// <inheritdoc/>
        public IChannelSourceHandle GetWriteChannelSource(CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            var server = _cluster.SelectServerAndPinIfNeeded(_session, WritableServerSelector.Instance, cancellationToken);
            return GetChannelSourceHelper(server);
        }

        /// <inheritdoc/>
        public async Task<IChannelSourceHandle> GetWriteChannelSourceAsync(CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            var server = await _cluster.SelectServerAndPinIfNeededAsync(_session, WritableServerSelector.Instance, cancellationToken).ConfigureAwait(false);
            return GetChannelSourceHelper(server);
        }

        private IChannelSourceHandle GetChannelSourceHelper(IServer server)
        {
            return new ChannelSourceHandle(new ServerChannelSource(server, _session.Fork()));
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (!_disposed)
            {
                _session.Dispose();
                _disposed = true;
            }
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
    }
}
