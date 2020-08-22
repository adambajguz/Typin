namespace Typin.Tests.Commands.Invalid
{
    using Typin.Attributes;

    [Command("cmd")]
    public class DuplicateOptionShortNamesCommand : SelfSerializeCommandBase
    {
        [CommandOption('x')]
        public string? OptionA { get; set; }

        [CommandOption('x')]
        public string? OptionB { get; set; }
    }
}