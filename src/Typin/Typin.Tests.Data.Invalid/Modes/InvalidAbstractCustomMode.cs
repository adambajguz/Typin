﻿namespace Typin.Tests.Data.Modes.Invalid
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Typin.Console;

    public abstract class InvalidAbstractCustomMode : ICliMode
    {
        private readonly ICliApplicationLifetime _applicationLifetime;
        private readonly IConsole _console;

        public InvalidAbstractCustomMode(ICliApplicationLifetime applicationLifetime, IConsole console)
        {
            _applicationLifetime = applicationLifetime;
            _console = console;
        }

        public async ValueTask<int> ExecuteAsync(IEnumerable<string> commandLineArguments, ICliCommandExecutor executor)
        {
            await _console.Output.WriteLineAsync(nameof(InvalidCustomMode));

            int exitCode = await executor.ExecuteCommandAsync(commandLineArguments);
            _applicationLifetime.RequestStop();

            await _console.Output.WriteLineAsync(nameof(InvalidCustomMode) + "END");

            return exitCode;
        }
    }
}
