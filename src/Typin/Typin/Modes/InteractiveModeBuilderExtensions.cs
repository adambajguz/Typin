namespace Typin.Modes
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// <see cref="CliApplicationBuilder"/> interactive mode configuration extensions.
    /// </summary>
    public static class InteractiveModeBuilderExtensions
    {
        /// <summary>
        /// Adds a direct mode to the application.
        /// </summary>
        public static CliApplicationBuilder UseInteractiveMode(this CliApplicationBuilder builder, bool asStartup = false, Action<InteractiveModeSettings>? configuration = null)
        {
            builder.RegisterMode<InteractiveMode>(asStartup);

            configuration ??= (InteractiveModeSettings cfg) => { };
            builder.ConfigureServices((IServiceCollection sc) => sc.Configure(configuration));

            return builder;
        }
    }
}
