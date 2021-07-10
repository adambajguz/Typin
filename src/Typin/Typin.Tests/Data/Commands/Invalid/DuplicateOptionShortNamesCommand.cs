namespace Typin.Tests.Data.Commands.Invalid
{
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.Commands;

    [Command("cmd")]
    public class DuplicateOptionShortNamesCommand : SelfSerializeCommandBase
    {
        [CommandOption('x')]
        public string? OptionA { get; init; }

        [CommandOption('x')]
        public string? OptionB { get; init; }

        public DuplicateOptionShortNamesCommand(IConsole console) : base(console)
        {

        }
    }
}