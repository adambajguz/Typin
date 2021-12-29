namespace Typin.Tests.Data.Invalid.Modes
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Console;
    using Typin.Modes;

    public abstract class InvalidAbstractCustomMode : ICliMode
    {
        private readonly IConsole _console;

        public InvalidAbstractCustomMode(IConsole console)
        {
            _console = console;
        }

        public async Task<int> ExecuteAsync(CancellationToken cancellationToken)
        {
            await _console.Output.WriteLineAsync(nameof(InvalidCustomMode));


            await _console.Output.WriteLineAsync(nameof(InvalidCustomMode) + "END");

            return 0;
        }
    }
}
