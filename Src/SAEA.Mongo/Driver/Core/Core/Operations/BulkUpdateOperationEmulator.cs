﻿/* Copyright 2010-present MongoDB Inc.
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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SAEA.Mongo.Bson;
using SAEA.Mongo.Bson.IO;
using SAEA.Mongo.Driver.Core.Bindings;
using SAEA.Mongo.Driver.Core.Connections;
using SAEA.Mongo.Driver.Core.Misc;
using SAEA.Mongo.Driver.Core.Operations.ElementNameValidators;
using SAEA.Mongo.Driver.Core.WireProtocol;
using SAEA.Mongo.Driver.Core.WireProtocol.Messages.Encoders;

namespace SAEA.Mongo.Driver.Core.Operations
{
    internal class BulkUpdateOperationEmulator : BulkUnmixedWriteOperationEmulatorBase<UpdateRequest>
    {
        // constructors
        public BulkUpdateOperationEmulator(
            CollectionNamespace collectionNamespace,
            IEnumerable<UpdateRequest> requests,
            MessageEncoderSettings messageEncoderSettings)
            : base(collectionNamespace, requests, messageEncoderSettings)
        {
        }

        // methods
        protected override WriteConcernResult ExecuteProtocol(IChannelHandle channel, UpdateRequest request, CancellationToken cancellationToken)
        {
            if (request.Collation != null)
            {
                throw new NotSupportedException("BulkUpdateOperationEmulator does not support collations.");
            }
            if (request.ArrayFilters != null)
            {
                throw new NotSupportedException("BulkUpdateOperationEmulator does not support arrayFilters.");
            }

            return channel.Update(
                CollectionNamespace,
                MessageEncoderSettings,
                WriteConcern,
                request.Filter,
                request.Update,
                ElementNameValidatorFactory.ForUpdateType(request.UpdateType),
                request.IsMulti,
                request.IsUpsert,
                cancellationToken);
        }

        protected override Task<WriteConcernResult> ExecuteProtocolAsync(IChannelHandle channel, UpdateRequest request, CancellationToken cancellationToken)
        {
            if (request.Collation != null)
            {
                throw new NotSupportedException("BulkUpdateOperationEmulator does not support collations.");
            }
            if (request.ArrayFilters != null)
            {
                throw new NotSupportedException("BulkUpdateOperationEmulator does not support arrayFilters.");
            }

            return channel.UpdateAsync(
                CollectionNamespace,
                MessageEncoderSettings,
                WriteConcern,
                request.Filter,
                request.Update,
                ElementNameValidatorFactory.ForUpdateType(request.UpdateType),
                request.IsMulti,
                request.IsUpsert,
                cancellationToken);
        }
    }
}
