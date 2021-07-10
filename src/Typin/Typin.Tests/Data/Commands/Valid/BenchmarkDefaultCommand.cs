namespace Typin.Tests.Data.Commands.Valid
{
    using Typin.Attributes;
    using Typin.Console;

    [Command]
    public class BenchmarkDefaultCommand : SelfSerializeCommandBase
    {
        [CommandOption("str", 's')]
        public string? StrOption { get; init; }

        [CommandOption("int", 'i')]
        public int IntOption { get; init; }

        [CommandOption("bool", 'b')]
        public bool BoolOption { get; init; }

        public BenchmarkDefaultCommand(IConsole console) : base(console)
        {

        }
    }
}
