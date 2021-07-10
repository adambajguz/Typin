namespace Typin.Tests.Data.Commands.Invalid
{
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.Commands;

    [Command("cmd")]
    public class NonLetterOptionShortName0Command : SelfSerializeCommandBase
    {
        [CommandOption('0')]
        public string? Apples { get; init; }

        public NonLetterOptionShortName0Command(IConsole console) : base(console)
        {

        }
    }

    [Command("cmd")]
    public class NonLetterOptionShortName1Command : SelfSerializeCommandBase
    {
        [CommandOption('=')]
        public string? Apples { get; init; }

        public NonLetterOptionShortName1Command(IConsole console) : base(console)
        {

        }
    }

    [Command("cmd")]
    public class NonLetterOptionShortName2Command : SelfSerializeCommandBase
    {
        [CommandOption('-')]
        public string? Apples { get; init; }

        public NonLetterOptionShortName2Command(IConsole console) : base(console)
        {

        }
    }

    [Command("cmd")]
    public class NonLetterOptionShortName3Command : SelfSerializeCommandBase
    {
        [CommandOption('~')]
        public string? Apples { get; init; }

        public NonLetterOptionShortName3Command(IConsole console) : base(console)
        {

        }
    }
}