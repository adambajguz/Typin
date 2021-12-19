namespace Typin.Tests.Data.Invalid.Commands
{
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.Common.Commands;

    [Command("cmd")]
    public class NonLetterOptionShortName0Command : SelfSerializeCommandBase
    {
        [Option('0')]
        public string? Apples { get; init; }

        public NonLetterOptionShortName0Command(IConsole console) : base(console)
        {

        }
    }

    [Command("cmd")]
    public class NonLetterOptionShortName1Command : SelfSerializeCommandBase
    {
        [Option('=')]
        public string? Apples { get; init; }

        public NonLetterOptionShortName1Command(IConsole console) : base(console)
        {

        }
    }

    [Command("cmd")]
    public class NonLetterOptionShortName2Command : SelfSerializeCommandBase
    {
        [Option('-')]
        public string? Apples { get; init; }

        public NonLetterOptionShortName2Command(IConsole console) : base(console)
        {

        }
    }

    [Command("cmd")]
    public class NonLetterOptionShortName3Command : SelfSerializeCommandBase
    {
        [Option('~')]
        public string? Apples { get; init; }

        public NonLetterOptionShortName3Command(IConsole console) : base(console)
        {

        }
    }
}