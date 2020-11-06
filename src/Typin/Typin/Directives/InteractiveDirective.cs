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
        /// <inheritdoc/>
        public bool ContinueExecution => true;

        private readonly ICliModeSwitcher _modeSwitcher;

        /// <summary>
        /// Initializes an instance of <see cref="InteractiveDirective"/>.
        /// </summary>
        public InteractiveDirective(ICliModeSwitcher cliContext)
        {
            _modeSwitcher = cliContext;
        }

        /// <inheritdoc/>
        public ValueTask HandleAsync(IConsole console)
        {
            _modeSwitcher.RequestMode<InteractiveMode>();

            return default;
        }
    }
}
