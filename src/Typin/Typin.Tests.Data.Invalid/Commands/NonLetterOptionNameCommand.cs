namespace Typin.Tests.Data.Invalid.Commands
{
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.Common.Commands;

    [Command("cmd")]
    public class NonLetterOptionName0Command : SelfSerializeCommandBase
    {
        [Option("0a")]
        public string? Apples1 { get; init; }

        public NonLetterOptionName0Command(IConsole console) : base(console)
        {

        }
    }

    [Command("cmd")]
    public class NonLetterOptionName1Command : SelfSerializeCommandBase
    {
        [Option("=a")]
        public string? Apples2 { get; init; }

        public NonLetterOptionName1Command(IConsole console) : base(console)
        {

        }
    }

    [Command("cmd")]
    public class NonLetterOptionName2Command : SelfSerializeCommandBase
    {
        [Option("==a")]
        public string? Apples3 { get; init; }

        public NonLetterOptionName2Command(IConsole console) : base(console)
        {

        }
    }

    [Command("cmd")]
    public class NonLetterOptionName3Command : SelfSerializeCommandBase
    {
        [Option("+ag")]
        public string? Apples3 { get; init; }

        public NonLetterOptionName3Command(IConsole console) : base(console)
        {

        }
    }
}