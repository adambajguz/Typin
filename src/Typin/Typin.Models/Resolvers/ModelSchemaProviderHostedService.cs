namespace Typin.Models.Resolvers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// A hosted service used to resolve all model schema during startup.
    /// </summary>
    internal sealed class ModelSchemaProviderHostedService : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        /// <summary>
        /// Initializes a new instance of <see cref="ModelSchemaProviderHostedService"/>.
        /// </summary>
        public ModelSchemaProviderHostedService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        /// <inheritdoc/>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await using AsyncServiceScope scope = _serviceScopeFactory.CreateAsyncScope();
            IModelSchemaProvider provider = scope.ServiceProvider.GetRequiredService<IModelSchemaProvider>();

            await provider.ReloadAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
