namespace Typin.Tests.Data.Invalid.DynamicCommands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Typin.Attributes;
    using Typin.Commands;
    using Typin.Console;
    using Typin.Models.Collections;

    public class InvalidDynamicCommand : ICommand
    {
        private readonly IConsole _console;

        [Parameter(0)]
        public string Param0 { get; init; } = string.Empty;

        [Option("opt0")]
        public string Opt0 { get; init; } = string.Empty;

        [Option("opt1")]
        public int Opt1 { get; init; }

        public IArgumentCollection Arguments { get; init; } = default!;

        public InvalidDynamicCommand(IConsole console)
        {
            _console = console;
        }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            _console.Output.WriteLine(JsonConvert.SerializeObject(this, Formatting.Indented));

            return default;
        }
    }
}
