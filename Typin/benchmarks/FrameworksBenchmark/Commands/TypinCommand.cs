namespace Typin.Benchmarks.FrameworksComparison.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Commands;
    using Typin.Commands.Attributes;
    using Typin.Models.Attributes;

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