namespace Typin.Benchmarks.MultiCommand.TypinCommands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Commands;
    using Typin.Models.Attributes;

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