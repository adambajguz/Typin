namespace Typin.Tests.Data.Commands.Invalid
{
    using Typin.Attributes;
    using Typin.Tests.Data.Commands;

    [Command("cmd")]
    public class NonLetterOptionName0Command : SelfSerializeCommandBase
    {
        [CommandOption("0a")]
        public string? Apples1 { get; init; }
    }

    [Command("cmd")]
    public class NonLetterOptionName1Command : SelfSerializeCommandBase
    {
        [CommandOption("=a")]
        public string? Apples2 { get; init; }
    }

    [Command("cmd")]
    public class NonLetterOptionName2Command : SelfSerializeCommandBase
    {
        [CommandOption("==a")]
        public string? Apples3 { get; init; }
    }

    [Command("cmd")]
    public class NonLetterOptionName3Command : SelfSerializeCommandBase
    {
        [CommandOption("+ag")]
        public string? Apples3 { get; init; }
    }
}