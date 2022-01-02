namespace Typin.Commands.Resolvers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Typin.Commands.Schemas;

    /// <summary>
    /// A hosted service used to resolve all command schema during startup.
    /// </summary>
    internal sealed class CommandSchemaProviderHostedService : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        /// <summary>
        /// Initializes a new instance of <see cref="CommandSchemaProviderHostedService"/>.
        /// </summary>
        public CommandSchemaProviderHostedService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        /// <inheritdoc/>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await using AsyncServiceScope scope = _serviceScopeFactory.CreateAsyncScope();
            ICommandSchemaProvider provider = scope.ServiceProvider.GetRequiredService<ICommandSchemaProvider>();

            await provider.ReloadAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
