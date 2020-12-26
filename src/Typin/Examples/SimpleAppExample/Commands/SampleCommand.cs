namespace SimpleAppExample.Commands
{
    using System.Text.Json;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command]
    public class SampleCommand : ICommand
    {
        [CommandParameter(0)]
        public int? ParamB { get; init; }

        [CommandOption("str", 's')]
        public string? StrOption { get; init; }

        [CommandOption("int", 'i')]
        public int IntOption { get; init; }

        [CommandOption("bool", 'b')]
        public bool BoolOption { get; init; }

        [CommandOption('v')]
        public bool VOption { get; init; }

        [CommandOption('x')]
        public bool XOption { get; init; }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await console.Output.WriteLineAsync(JsonSerializer.Serialize(this));
        }
    }
}