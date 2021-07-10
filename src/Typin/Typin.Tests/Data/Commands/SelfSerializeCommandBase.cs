namespace Typin.Tests.Data.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Console;
    using Typin.Tests.Extensions;

    public abstract class SelfSerializeCommandBase : ICommand
    {
        private readonly IConsole _console;

        protected SelfSerializeCommandBase(IConsole console)
        {
            _console = console;
        }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            string json = this.SerializeJson();
            _console.Output.WriteLine(json);

            return default;
        }
    }
}