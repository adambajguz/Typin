namespace Typin.Modes
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Commands;
    using Typin.Directives;

    /// <summary>
    /// <see cref="CliApplicationBuilder"/> interactive mode configuration extensions.
    /// </summary>
    public static class InteractiveModeBuilderExtensions
    {
        /// <summary>
        /// Adds an interactive mode to the application (enabled with [interactive] directive or `interactive` command).
        /// By default this adds [interactive], [default], [>], [.], and [..], as well as `interactive` command and advanced command input.
        /// </summary>
        public static CliApplicationBuilder UseInteractiveMode(this CliApplicationBuilder builder,
                                                               bool asStartup = false,
                                                               Action<InteractiveModeOptions>? options = null,
                                                               InteractiveModeBuilderSettings? builderSettings = null)
        {
            builderSettings ??= new InteractiveModeBuilderSettings();

            builder.RegisterMode<InteractiveMode>(asStartup);

            options ??= (InteractiveModeOptions cfg) => { };
            builder.ConfigureServices((IServiceCollection sc) => sc.Configure(options));

            builder.AddDirective<DefaultDirective>();

            if (builderSettings.AddInteractiveCommand)
            {
                builder.AddCommand<InteractiveCommand>();
            }

            if (builderSettings.AddInteractiveDirective)
            {
                builder.AddDirective<InteractiveDirective>();
            }

            if (builderSettings.AddScopeDirectives)
            {
                builder.AddDirective<ScopeDirective>();
                builder.AddDirective<ScopeResetDirective>();
                builder.AddDirective<ScopeUpDirective>();
            }

            return builder;
        }
    }
}
