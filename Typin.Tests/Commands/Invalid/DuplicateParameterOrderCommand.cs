namespace Typin.Tests.Commands.Invalid
{
    using Typin.Attributes;

    [Command("cmd")]
    public class DuplicateParameterOrderCommand : SelfSerializeCommandBase
    {
        [CommandParameter(13)]
        public string? ParamA { get; set; }

        [CommandParameter(13)]
        public string? ParamB { get; set; }
    }
}