namespace Typin.Tests.Data.Commands.Invalid
{
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.Commands;

    [Command("cmd")]
    public class ConflictWithHelpOptionCommand : SelfSerializeCommandBase
    {
        [CommandOption("option-h", 'h')]
        public string? OptionH { get; init; }

        public ConflictWithHelpOptionCommand(IConsole console) : base(console)
        {

        }
    }
}