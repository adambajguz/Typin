namespace Typin.Tests.Data.Commands.Invalid
{
    using Typin.Attributes;
    using Typin.Tests.Data.Commands;

    [Command("cmd")]
    public class NonLetterOptionShortName0Command : SelfSerializeCommandBase
    {
        [CommandOption('0')]
        public string? Apples { get; init; }
    }

    [Command("cmd")]
    public class NonLetterOptionShortName1Command : SelfSerializeCommandBase
    {
        [CommandOption('=')]
        public string? Apples { get; init; }
    }

    [Command("cmd")]
    public class NonLetterOptionShortName2Command : SelfSerializeCommandBase
    {
        [CommandOption('-')]
        public string? Apples { get; init; }
    }

    [Command("cmd")]
    public class NonLetterOptionShortName3Command : SelfSerializeCommandBase
    {
        [CommandOption('~')]
        public string? Apples { get; init; }
    }
}