namespace Typin.Tests.Data.Commands.Invalid
{
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.Commands;

    [Command("cmd")]
    public class InvalidOptionShortNameCommand : SelfSerializeCommandBase
    {
        [CommandOption('1')]
        public string? Apples { get; init; }

        [CommandOption("fruits", '0')]
        public string? Oranges { get; init; }

        public InvalidOptionShortNameCommand(IConsole console) : base(console)
        {

        }
    }
}