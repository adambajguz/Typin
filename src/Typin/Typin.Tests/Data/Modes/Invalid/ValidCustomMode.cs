namespace Typin.Modes
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Typin.Console;

    public class InvalidCustomMode
    {
        private readonly ICliApplicationLifetime _applicationLifetime;
        private readonly IConsole _console;

        public InvalidCustomMode(ICliApplicationLifetime applicationLifetime, IConsole console)
        {
            _applicationLifetime = applicationLifetime;
            _console = console;
        }

        public async ValueTask<int> ExecuteAsync(IReadOnlyList<string> commandLineArguments, ICliCommandExecutor executor)
        {
            await _console.Output.WriteLineAsync(nameof(InvalidCustomMode));

            int exitCode = await executor.ExecuteCommandAsync(commandLineArguments);
            _applicationLifetime.RequestStop();

            await _console.Output.WriteLineAsync(nameof(InvalidCustomMode) + "END");

            return exitCode;
        }
    }
}
