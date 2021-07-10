namespace Typin.Tests.Data.Commands.Valid
{
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.Commands;

    [Command("cmd")]
    public class WithSingleRequiredOptionCommand : SelfSerializeCommandBase
    {
        [CommandOption("opt-a")]
        public string? OptA { get; init; }

        [CommandOption("opt-b", IsRequired = true)]
        public string? OptB { get; init; }

        public WithSingleRequiredOptionCommand(IConsole console) : base(console)
        {

        }
    }
}