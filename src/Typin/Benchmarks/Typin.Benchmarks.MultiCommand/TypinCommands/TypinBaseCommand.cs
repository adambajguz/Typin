namespace Typin.Benchmarks.MultiCommand.TypinCommands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Commands;

    public abstract class TypinBaseCommand : ICommand
    {
        [Option("str", 's')]
        public string? StrOption { get; set; }

        [Option("int", 'i')]
        public int IntOption { get; set; }

        [Option("bool", 'b')]
        public bool BoolOption { get; set; }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            return default;
        }
    }
}