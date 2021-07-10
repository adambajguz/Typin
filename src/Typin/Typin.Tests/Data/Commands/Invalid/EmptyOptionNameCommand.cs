namespace Typin.Tests.Data.Commands.Invalid
{
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.Commands;

    [Command("cmd")]
    public class EmptyOptionNameCommand : SelfSerializeCommandBase
    {
        [CommandOption]
        public string? Apples { get; init; }

        [CommandOption]
        public string? Blackberries { get; init; }

        [CommandOption]
        public string? WestIndianCherry { get; init; }

        [CommandOption]
        public string? CoconutMeat_Or_PitayaDragonfruit { get; init; }

        [CommandOption]
        public string? CoconutMeat_or_Pitaya { get; init; }

        public EmptyOptionNameCommand(IConsole console) : base(console)
        {

        }
    }
}