namespace Typin.Tests.Data.Modes.Invalid
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Console;

    public abstract class InvalidAbstractCustomMode : ICliMode
    {
        private readonly IConsole _console;

        public InvalidAbstractCustomMode(IConsole console)
        {
            _console = console;
        }

        public async ValueTask<int> ExecuteAsync(CancellationToken cancellationToken)
        {
            await _console.Output.WriteLineAsync(nameof(InvalidCustomMode));


            await _console.Output.WriteLineAsync(nameof(InvalidCustomMode) + "END");

            return 0;
        }
    }
}
