namespace Typin.Tests.Data.Valid.Commands
{
    using Typin.Commands.Attributes;
    using Typin.Console;
    using Typin.Models.Attributes;
    using Typin.Tests.Data.Common.Commands;

    [Command(nameof(WithSingleRequiredOptionCommand))]
    public class WithSingleRequiredOptionCommand : SelfSerializeCommandBase
    {
        [Option("opt-a")]
        public string? OptA { get; init; }

        [Option("opt-b", IsRequired = true)]
        public string? OptB { get; init; }

        public WithSingleRequiredOptionCommand(IConsole console) : base(console)
        {

        }
    }
}