namespace Typin.Tests.Commands.Invalid
{
    using Typin.Attributes;

    [Command("cmd")]
    public class DuplicateParameterNameCommand : SelfSerializeCommandBase
    {
        [CommandParameter(0, Name = "param")]
        public string? ParamA { get; set; }

        [CommandParameter(1, Name = "param")]
        public string? ParamB { get; set; }
    }
}