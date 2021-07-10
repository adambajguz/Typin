namespace Typin.Benchmarks.MultiCommand.TypinCommands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Attributes;

    public abstract class TypinBaseCommand : ICommand
    {
        [CommandOption("str", 's')]
        public string? StrOption { get; set; }

        [CommandOption("int", 'i')]
        public int IntOption { get; set; }

        [CommandOption("bool", 'b')]
        public bool BoolOption { get; set; }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            return default;
        }
    }
}