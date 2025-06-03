namespace Typin.Tests.Data.Invalid.Commands
{
    using Typin.Console;
    using Typin.Models.Attributes;
    using Typin.Schemas.Attributes;
    using Typin.Tests.Data.Common.Commands;

    [Alias(nameof(NonLetterOptionShortName0Command))]
    public class NonLetterOptionShortName0Command : SelfSerializeCommandBase
    {
        [Option('0')]
        public string? Apples { get; init; }

        public NonLetterOptionShortName0Command(IConsole console) : base(console)
        {

        }
    }

    [Alias(nameof(NonLetterOptionShortName1Command))]
    public class NonLetterOptionShortName1Command : SelfSerializeCommandBase
    {
        [Option('=')]
        public string? Apples { get; init; }

        public NonLetterOptionShortName1Command(IConsole console) : base(console)
        {

        }
    }

    [Alias(nameof(NonLetterOptionShortName2Command))]
    public class NonLetterOptionShortName2Command : SelfSerializeCommandBase
    {
        [Option('-')]
        public string? Apples { get; init; }

        public NonLetterOptionShortName2Command(IConsole console) : base(console)
        {

        }
    }

    [Alias(nameof(NonLetterOptionShortName3Command))]
    public class NonLetterOptionShortName3Command : SelfSerializeCommandBase
    {
        [Option('~')]
        public string? Apples { get; init; }

        public NonLetterOptionShortName3Command(IConsole console) : base(console)
        {

        }
    }
}