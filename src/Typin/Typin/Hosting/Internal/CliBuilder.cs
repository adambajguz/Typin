namespace Typin.Hosting.Internal
{
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Hosting;

    /// <summary>
    /// CLI builder.
    /// </summary>
    internal class CliBuilder : ICliBuilder
    {
        /// <inheritdoc/>
        public IServiceCollection Services { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="CliBuilder"/>.
        /// </summary>
        /// <param name="services"></param>
        public CliBuilder(IServiceCollection services)
        {
            Services = services;
        }
    }
}
