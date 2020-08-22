namespace Typin.Tests.Commands.Valid
{
    using Typin.Attributes;

    [Command("cmd")]
    public class WithSingleParameterCommand : SelfSerializeCommandBase
    {
        [CommandParameter(0)]
        public string? ParamA { get; set; }
    }
}