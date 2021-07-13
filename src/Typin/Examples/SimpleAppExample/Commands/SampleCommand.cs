namespace SimpleAppExample.Commands
{
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command]
    public class SampleCommand : ICommand
    {
        private readonly IConsole _console;

        [Parameter(0)]
        public int? ParamB { get; init; }

        [Option("str", 's')]
        public string? StrOption { get; init; }

        [Option("int", 'i')]
        public int IntOption { get; init; }

        [Option("bool", 'b')]
        public bool BoolOption { get; init; }

        [Option('v')]
        public bool VOption { get; init; }

        [Option('x')]
        public bool XOption { get; init; }

        public SampleCommand(IConsole console)
        {
            _console = console;
        }

        public async ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            await _console.Output.WriteLineAsync(JsonSerializer.Serialize(this));
        }
    }
}