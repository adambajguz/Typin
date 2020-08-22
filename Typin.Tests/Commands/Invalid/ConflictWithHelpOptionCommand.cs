namespace Typin.Tests.Commands.Invalid
{
    using Typin.Attributes;
    using Typin.Tests.Commands;

    [Command("cmd")]
    public class ConflictWithHelpOptionCommand : SelfSerializeCommandBase
    {
        [CommandOption("option-h", 'h')]
        public string? OptionH { get; set; }
    }
}