namespace Typin.Plugins.Scopes
{
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using PackSite.Library.Pipelining;
    using Typin.Hosting;

    /// <summary>
    /// Plugin registration extensions.
    /// </summary>
    public static class PluginRegistrationExtensions
    {
        /// <summary>
        /// Adds support for scopes plugin.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ICliBuilder AddScopes(this ICliBuilder builder)
        {
            builder.Services.TryAddSingleton<IScopeManager, ScopeManager>();

            return builder;
        }

        /// <summary>
        /// Adds support for scopes plugin.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IPipelineBuilder<CliContext> AddScopes(this IPipelineBuilder<CliContext> builder)
        {
            return builder.Add<ScopeHandler>();
        }
    }
}
