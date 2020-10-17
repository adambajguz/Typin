namespace Typin.Tests.Data.Commands.Valid
{
    using Typin.Attributes;

    [Command]
    public class BenchmarkDefaultCommand : SelfSerializeCommandBase
    {
        [CommandOption("str", 's')]
        public string? StrOption { get; set; }

        [CommandOption("int", 'i')]
        public int IntOption { get; set; }

        [CommandOption("bool", 'b')]
        public bool BoolOption { get; set; }
    }
}
