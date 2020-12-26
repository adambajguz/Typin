namespace Typin.Tests.Data.Commands.Invalid
{
    using Typin.Attributes;
    using Typin.Tests.Data.Commands;

    [Command("cmd")]
    public class ConflictWithHelpOptionCommand : SelfSerializeCommandBase
    {
        [CommandOption("option-h", 'h')]
        public string? OptionH { get; init; }
    }
}