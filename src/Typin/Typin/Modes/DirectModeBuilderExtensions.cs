namespace Typin.Modes
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// <see cref="CliApplicationBuilder"/> direct mode configuration extensions.
    /// </summary>
    public static class DirectModeBuilderExtensions
    {
        /// <summary>
        /// Adds a direct mode to the application.
        /// </summary>
        public static CliApplicationBuilder UseDirectMode(this CliApplicationBuilder builder, bool asStartup = false, Action<DirectModeSettings>? configuration = null)
        {
            builder.RegisterMode<DirectMode>(asStartup);

            configuration ??= (DirectModeSettings cfg) => { };
            builder.ConfigureServices((IServiceCollection sc) => sc.Configure(configuration));

            return builder;
        }
    }
}
