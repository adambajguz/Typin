namespace Typin.Modes.Interactive.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Commands;
    using Typin.Modes;
    using Typin.Modes.Interactive.Directives;

    /// <summary>
    /// If application runs in interactive mode (using the interactive command or [interactive] directive), it is possible to execute multiple commands in one processes.
    /// The application will ran in loop, constantly asking user for command input.
    /// This is useful for situations when it is necessary to execute multiple commands (since you don't have to constantly type dotnet ...).
    /// Furthermore, application context can be shared, which is useful when you have a db connection or startup takes very long.
    /// </summary>
    [Command(InteractiveOnlyDirectives.Interactive, Description = "Starts an interactive mode.",
             ExcludedModes = new[] { typeof(InteractiveMode) })]
    public sealed class InteractiveCommand : ICommand
    {
        private readonly ICliModeSwitcher _cliModeSwitcher;

        /// <summary>
        /// Initializes an instance of <see cref="InteractiveCommand"/>.
        /// </summary>
        public InteractiveCommand(ICliModeSwitcher cliModeSwitcher)
        {
            _cliModeSwitcher = cliModeSwitcher;
        }

        /// <inheritdoc/>
        public async ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            await _cliModeSwitcher.WithModeAsync<InteractiveMode>(async (mode, ct) =>
            {
                await mode.ExecuteAsync(ct);

            }, cancellationToken);
        }
    }
}
