// ------------------------------------------------------------------------
// Copyright 2022 The Dapr Authors
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ------------------------------------------------------------------------

using System;

namespace Dapr.Client
{
    /// <summary>
    /// Class representing the response from a TryLock API call.
    /// </summary>
    public class TryLockResponse :  IDisposable
    {
        /// <summary>
        /// The success value of the tryLock API call
        /// </summary>
        public bool Success { get; }
        /// <summary>
        /// The resourceId required to unlock the lock
        /// </summary>
        public string ResourceId { get; }
         /// <summary>
        /// The LockOwner required to unlock the lock
        /// </summary>
        public string LockOwner { get; }
         /// <summary>
        /// The StoreName required to unlock the lock
        /// </summary>
        public string StoreName { get; }

        /// <summary>
        /// Constructor for a TryLockResponse.
        /// </summary>
        /// <param name="success">The success value that is returned in the TryLock call.</param>
        public TryLockResponse(bool success)
        {
            this.Success = success;
        }

        /// <inheritdoc />
        public void Dispose() {
            using (var client = new DaprClientBuilder().Build()) {
                client.Unlock(ResourceId, LockOwner, StoreName);
            }
        }

        /// <summary>
        /// Disposes the resources associated with the object.
        /// </summary>
        /// <param name="disposing"><c>true</c> if called by a call to the <c>Dispose</c> method; otherwise false.</param>
        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
