namespace Typin.Directives.Resolvers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Typin.Directives.Schemas;

    /// <summary>
    /// A hosted service used to resolve all directive schema during startup.
    /// </summary>
    internal sealed class DirectiveSchemaProviderHostedService : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        /// <summary>
        /// Initializes a new instance of <see cref="DirectiveSchemaProviderHostedService"/>.
        /// </summary>
        public DirectiveSchemaProviderHostedService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        /// <inheritdoc/>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await using AsyncServiceScope scope = _serviceScopeFactory.CreateAsyncScope();
            IDirectiveSchemaProvider provider = scope.ServiceProvider.GetRequiredService<IDirectiveSchemaProvider>();

            await provider.ReloadAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
