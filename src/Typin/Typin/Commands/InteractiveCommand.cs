namespace Typin.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Directives;
    using Typin.Modes;

    /// <summary>
    /// If application runs in interactive mode (using the interactive command or [interactive] directive), it is possible to execute multiple commands in one processes.
    /// The application will ran in loop, constantly asking user for command input.
    /// This is useful for situations when it is necessary to execute multiple commands (since you don't have to constantly type dotnet ...).
    /// Furthermore, application context can be shared, which is useful when you have a db connection or startup takes very long.
    /// </summary>
    [Command(BuiltInDirectives.Interactive, Description = "Starts an interactive mode.",
             ExcludedModes = new[] { typeof(InteractiveMode) })]
    public sealed class InteractiveCommand : ICommand
    {
        private readonly ICliApplicationLifetime _applicationLifetime;

        /// <summary>
        /// Initializes an instance of <see cref="InteractiveDirective"/>.
        /// </summary>
        public InteractiveCommand(ICliApplicationLifetime applicationLifetime)
        {
            _applicationLifetime = applicationLifetime;
        }

        /// <inheritdoc/>
        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            _applicationLifetime.RequestMode<InteractiveMode>();

            return default;
        }
    }
}
