namespace Typin.SimpleDemo.Commands
{
    using System.Threading.Tasks;
    using Typin.Attributes;

    [Command]
    public class TypinBenchmarkCommand : ICommand
    {
        [CommandOption("str", 's')]
        public string? StrOption { get; set; }

        [CommandOption("int", 'i')]
        public int IntOption { get; set; }

        [CommandOption("bool", 'b')]
        public bool BoolOption { get; set; }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await console.Output.WriteLineAsync($"Hello world {StrOption} {IntOption} {BoolOption}");
        }
    }
}