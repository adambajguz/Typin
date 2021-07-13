namespace Typin.Benchmarks.FrameworksComparison.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Attributes;

    [Command]
    public class TypinCommand : ICommand
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