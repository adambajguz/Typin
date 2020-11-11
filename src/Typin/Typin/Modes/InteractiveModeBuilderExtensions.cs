namespace Typin.Modes
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Directives;

    /// <summary>
    /// <see cref="CliApplicationBuilder"/> interactive mode configuration extensions.
    /// </summary>
    public static class InteractiveModeBuilderExtensions
    {
        /// <summary>
        /// Adds a direct mode to the application (enabled with [interactive] directive).
        /// By default this adds [default], [>], [.], and [..] and advanced command input.
        ///
        /// If you wish to add only [default] directive, set addScopeDirectives to false.
        /// </summary>
        public static CliApplicationBuilder UseInteractiveMode(this CliApplicationBuilder builder, bool asStartup = false, Action<InteractiveModeSettings>? configuration = null, bool addScopeDirectives = true)
        {
            builder.RegisterMode<InteractiveMode>(asStartup);

            configuration ??= (InteractiveModeSettings cfg) => { };
            builder.ConfigureServices((IServiceCollection sc) => sc.Configure(configuration));

            builder.AddDirective<InteractiveDirective>();
            builder.AddDirective<DefaultDirective>();

            if (addScopeDirectives)
            {
                builder.AddDirective<ScopeDirective>();
                builder.AddDirective<ScopeResetDirective>();
                builder.AddDirective<ScopeUpDirective>();
            }

            return builder;
        }
    }
}
