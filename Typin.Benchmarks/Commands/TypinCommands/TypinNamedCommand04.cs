namespace Typin.Benchmarks.Commands.TypinCommands
{
    using System.Threading.Tasks;
    using Typin.Attributes;

    [Command("named-command04")]
    public class TypinNamedCommand04 : ICommand
    {
        [CommandOption("str", 's')]
        public string? StrOption { get; set; }

        [CommandOption("int", 'i')]
        public int IntOption { get; set; }

        [CommandOption("bool", 'b')]
        public bool BoolOption { get; set; }

        public ValueTask ExecuteAsync(IConsole console)
        {
            return default;
        }
    }
}