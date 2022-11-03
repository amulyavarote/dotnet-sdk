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
using Microsoft.DurableTask.Grpc;
using Microsoft.Extensions.DependencyInjection;
using Dapr.Workflow;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Contains extension methods for using Dapr Workflow with dependency injection.
    /// </summary>
    public static class WorkflowServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Dapr Workflow support to the service collection.
        /// </summary>
        /// <param name="serviceCollection">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configure">A delegate used to configure actor options and register workflow functions.</param>
        public static IServiceCollection AddWorkflow(
            this IServiceCollection serviceCollection,
            Action<WorkflowRuntimeOptions> configure)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            serviceCollection.AddSingleton<WorkflowRuntimeOptions>();
            serviceCollection.AddDaprClient();
            serviceCollection.AddSingleton(services => new WorkflowClient(services));

            serviceCollection.AddHostedService(services =>
            {
                DurableTaskGrpcWorker.Builder workerBuilder = DurableTaskGrpcWorker.CreateBuilder().UseServices(services);

                WorkflowRuntimeOptions options = services.GetRequiredService<WorkflowRuntimeOptions>();
                configure?.Invoke(options);

                workerBuilder.UseServices(services);

                workerBuilder.AddTasks(registry =>
                {
                    options.AddWorkflowsToRegistry(registry);
                });

                DurableTaskGrpcWorker worker = workerBuilder.Build();
                return worker;
            });

            return serviceCollection;
        }
    }
}

