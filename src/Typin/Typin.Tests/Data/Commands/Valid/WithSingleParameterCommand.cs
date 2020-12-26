namespace Typin.Tests.Data.Commands.Valid
{
    using Typin.Attributes;
    using Typin.Tests.Data.Commands;

    [Command("cmd")]
    public class WithSingleParameterCommand : SelfSerializeCommandBase
    {
        [CommandParameter(0)]
        public string? ParamA { get; init; }
    }
}