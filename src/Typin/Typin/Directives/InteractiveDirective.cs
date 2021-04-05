namespace Typin.Directives
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Modes;

    /// <summary>
    /// If application runs in interactive mode (using the interactive command or [interactive] directive), it is possible to execute multiple commands in one processes.
    /// The application will run in a loop, constantly asking user for command input.
    /// This is useful for situations when it is necessary to execute multiple commands (since you don't have to constantly type dotnet ...).
    /// Furthermore, application context can be shared, which is useful when you have a db connection or startup takes very long.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [Directive(BuiltInDirectives.Interactive, Description = "Executs a command, then starts an interactive mode.",
               ExcludedModes = new[] { typeof(InteractiveMode) })]
    public sealed class InteractiveDirective : IPipelinedDirective
    {
        private readonly ICliApplicationLifetime _applicationLifetime;

        /// <summary>
        /// Initializes an instance of <see cref="InteractiveDirective"/>.
        /// </summary>
        public InteractiveDirective(ICliApplicationLifetime cliContext)
        {
            _applicationLifetime = cliContext;
        }

        /// <inheritdoc/>
        public ValueTask OnInitializedAsync(CancellationToken cancellationToken)
        {
            return default;
        }

        /// <inheritdoc/>
        public async ValueTask HandleAsync(ICliContext context, CommandPipelineHandlerDelegate next, CancellationToken cancellationToken)
        {
            _applicationLifetime.RequestMode<InteractiveMode>();

            await next();
        }
    }
}
