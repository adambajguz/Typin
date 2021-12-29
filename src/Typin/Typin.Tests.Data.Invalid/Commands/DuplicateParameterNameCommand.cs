namespace Typin.Tests.Data.Invalid.Commands
{
    using Typin.Commands.Attributes;
    using Typin.Console;
    using Typin.Models.Attributes;
    using Typin.Tests.Data.Common.Commands;

    [Command("cmd")]
    public class DuplicateParameterNameCommand : SelfSerializeCommandBase
    {
        [Parameter(0, Name = "param")]
        public string? ParamA { get; init; }

        [Parameter(1, Name = "param")]
        public string? ParamB { get; init; }

        public DuplicateParameterNameCommand(IConsole console) : base(console)
        {

        }
    }
}