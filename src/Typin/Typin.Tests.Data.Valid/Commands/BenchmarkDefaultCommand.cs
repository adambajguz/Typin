namespace Typin.Tests.Data.Commands.Valid
{
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.Common.Commands;

    [Command]
    public class BenchmarkDefaultCommand : SelfSerializeCommandBase
    {
        [Option("str", 's')]
        public string? StrOption { get; init; }

        [Option("int", 'i')]
        public int IntOption { get; init; }

        [Option("bool", 'b')]
        public bool BoolOption { get; init; }

        public BenchmarkDefaultCommand(IConsole console) : base(console)
        {

        }
    }
}
