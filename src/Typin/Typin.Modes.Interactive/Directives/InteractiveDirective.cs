namespace Typin.Modes.Interactive.Directives
{
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin.Attributes;
    using Typin.Commands;
    using Typin.Directives;
    using Typin.Modes;

    /// <summary>
    /// If application runs in interactive mode (using the interactive command or [interactive] directive), it is possible to execute multiple commands in one processes.
    /// The application will run in a loop, constantly asking user for command input.
    /// This is useful for situations when it is necessary to execute multiple commands (since you don't have to constantly type dotnet ...).
    /// Furthermore, application context can be shared, which is useful when you have a db connection or startup takes very long.
    /// </summary>
    [Directive(InteractiveOnlyDirectives.Interactive, Description = "Executes a command, then starts an interactive mode.",
               ExcludedModes = new[] { typeof(InteractiveMode) })]
    public sealed class InteractiveDirective : IDirective //TODO: add directive hadnler
    {
        private readonly ICliModeSwitcher _cliModeSwitcher;
        private readonly ICommandExecutor _commandExecutor;

        /// <summary>
        /// Initializes an instance of <see cref="InteractiveDirective"/>.
        /// </summary>
        public InteractiveDirective(ICliModeSwitcher cliModeSwitcher,
                                    ICommandExecutor commandExecutor)
        {
            _cliModeSwitcher = cliModeSwitcher;
            _commandExecutor = commandExecutor;
        }

        /// <inheritdoc/>
        public async ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            await _cliModeSwitcher.WithModeAsync<InteractiveMode>(async (mode, ct) =>
            {
                string? commandLine = args.Input.Parsed?.WithoutDirective(InteractiveOnlyDirectives.Interactive).ToString();
                if (!string.IsNullOrWhiteSpace(commandLine))
                {
                    await _commandExecutor.ExecuteAsync(commandLine, cancellationToken: ct);
                }

                await mode.ExecuteAsync(ct);

            }, cancellationToken);
        }
    }
}
