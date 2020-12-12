namespace Typin.Tests.Data.Modes.Valid
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Console;

    public class ValidCustomMode : ICliMode
    {
        private readonly ICliApplicationLifetime _applicationLifetime;
        private readonly IConsole _console;

        public ValidCustomMode(ICliApplicationLifetime applicationLifetime, IConsole console)
        {
            _applicationLifetime = applicationLifetime;
            _console = console;
        }

        public async ValueTask<int> ExecuteAsync(IEnumerable<string> commandLineArguments, ICliCommandExecutor executor)
        {
            await _console.Output.WriteLineAsync(nameof(ValidCustomMode));

            int exitCode = await executor.ExecuteCommandAsync(commandLineArguments);
            _applicationLifetime.RequestStop();

            await _console.Output.WriteLineAsync(nameof(ValidCustomMode) + "END");

            return exitCode;
        }
    }
}
