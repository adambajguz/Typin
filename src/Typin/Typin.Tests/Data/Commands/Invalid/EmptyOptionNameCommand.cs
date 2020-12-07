namespace Typin.Tests.Data.Commands.Invalid
{
    using Typin.Attributes;
    using Typin.Tests.Data.Commands;

    [Command("cmd")]
    public class EmptyOptionNameCommand : SelfSerializeCommandBase
    {
        [CommandOption]
        public string? Apples { get; set; }

        [CommandOption]
        public string? Blackberries { get; set; }

        [CommandOption]
        public string? WestIndianCherry { get; set; }

        [CommandOption]
        public string? CoconutMeat_Or_PitayaDragonfruit { get; set; }

        [CommandOption]
        public string? CoconutMeat_or_Pitaya { get; set; }
    }
}