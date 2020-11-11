namespace Typin.Directives
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Modes;

    /// <summary>
    /// If application runs in interactive mode (using the [interactive] directive), it is possible to execute multiple commands in one processes.
    /// The application will ran in loop, constantly asking user for command input.
    /// This is useful for situations when it is necessary to execute multiple commands (since you don't have to constantly type dotnet ...).
    /// Furthermore, application context can be shared, which is useful when you have a db connection or startup takes very long.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [Directive(BuiltInDirectives.Interactive, Description = "Starts an interactive mode.")]
    public sealed class InteractiveDirective : IDirective
    {
        private readonly ICliApplicationLifetime _applicationLifetime;

        /// <inheritdoc/>
        public bool ContinueExecution => false;

        /// <summary>
        /// Initializes an instance of <see cref="InteractiveDirective"/>.
        /// </summary>
        public InteractiveDirective(ICliApplicationLifetime cliContext)
        {
            _applicationLifetime = cliContext;
        }

        /// <inheritdoc/>
        public ValueTask HandleAsync(IConsole console)
        {
            _applicationLifetime.RequestMode<InteractiveMode>();

            return default;
        }
    }
}
