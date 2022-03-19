namespace Typin.Modes.Interactive.Directives
{
    using System.ComponentModel;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin.Commands;
    using Typin.Directives;
    using Typin.Schemas.Attributes;

    /// <summary>
    /// If application runs in interactive mode (using the interactive command or [interactive] directive), it is possible to execute multiple commands in one processes.
    /// The application will run in a loop, constantly asking user for command input.
    /// This is useful for situations when it is necessary to execute multiple commands (since you don't have to constantly type dotnet ...).
    /// Furthermore, application context can be shared, which is useful when you have a db connection or startup takes very long.
    /// </summary>
    [Alias(InteractiveOnlyDirectives.Interactive)]
    [Description("Executes a command, then starts an interactive mode.")]
    public sealed class InteractiveDirective : IDirective //TODO: add directive handler
    {
        private sealed class Handler : IDirectiveHandler<InteractiveDirective>
        {
            private readonly ICliModeSwitcher _cliModeSwitcher;
            private readonly ICommandExecutor _commandExecutor;

            /// <summary>
            /// Initializes an instance of <see cref="InteractiveDirective"/>.
            /// </summary>
            public Handler(ICliModeSwitcher cliModeSwitcher,
                           ICommandExecutor commandExecutor)
            {
                _cliModeSwitcher = cliModeSwitcher;
                _commandExecutor = commandExecutor;
            }

            /// <inheritdoc/>
            public async ValueTask ExecuteAsync(DirectiveArgs<InteractiveDirective> args, StepDelegate next, CancellationToken cancellationToken = default)
            {
                //await _cliModeSwitcher.WithModeAsync<InteractiveMode>(async (mode, ct) =>
                //{
                //    TokensGroup<IDirectiveToken>? parsedInput = args.Context.Input.Tokens;

                //    if (parsedInput is not null)
                //    {
                //        parsedInput.Directives.RemoveAll(x => x.Matches(InteractiveOnlyDirectives.Interactive));

                //        if (parsedInput.GetRaw() is { Count: > 0 } arguments)
                //        {
                //            await _commandExecutor.ExecuteAsync(arguments, cancellationToken: ct);
                //        }
                //    }

                //    await mode.ExecuteAsync(ct);

                //}, cancellationToken);
            }
        }
    }
}
