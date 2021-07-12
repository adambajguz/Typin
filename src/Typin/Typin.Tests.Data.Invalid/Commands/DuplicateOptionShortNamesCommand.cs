namespace Typin.Tests.Data.Commands.Invalid
{
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.Common.Commands;

    [Command("cmd")]
    public class DuplicateOptionShortNamesCommand : SelfSerializeCommandBase
    {
        [Option('x')]
        public string? OptionA { get; init; }

        [Option('x')]
        public string? OptionB { get; init; }

        public DuplicateOptionShortNamesCommand(IConsole console) : base(console)
        {

        }
    }
}