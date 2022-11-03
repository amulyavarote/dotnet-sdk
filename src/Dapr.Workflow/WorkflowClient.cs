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
using System.Threading;
using System.Threading.Tasks;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Grpc;

namespace Dapr.Workflow
{
    /// <summary>
    /// Defines properties and methods for workflow client.
    /// </summary>
    public sealed class WorkflowClient : IAsyncDisposable
    {
        readonly DurableTaskClient innerClient;

        internal WorkflowClient(IServiceProvider? services = null)
        {
            DurableTaskGrpcClient.Builder builder = new();
            if (services != null)
            {
                builder.UseServices(services);
            }

            this.innerClient = builder.Build();
        }

        /// <summary>
        /// Method to schedule a new workflow.
        /// </summary>
        public Task<string> ScheduleNewWorkflowAsync(
            string name,
            string? instanceId = null,
            object? input = null,
            DateTime? startTime = null)
        {
            return this.innerClient.ScheduleNewOrchestrationInstanceAsync(name, instanceId, input, startTime);
        }

        /// <summary>
        /// Method to get the workflow metadata.
        /// </summary>
        public async Task<WorkflowMetadata> GetWorkflowMetadata(string instanceId, bool getInputsAndOutputs = false)
        {
            OrchestrationMetadata? metadata = await this.innerClient.GetInstanceMetadataAsync(
                instanceId,
                getInputsAndOutputs);
            return new WorkflowMetadata(metadata);
        }
        /// <summary>
        /// Method to implement interface member.
        /// </summary>
        public ValueTask DisposeAsync()
        {
            return ((IAsyncDisposable)this.innerClient).DisposeAsync();
        }
    }
}


