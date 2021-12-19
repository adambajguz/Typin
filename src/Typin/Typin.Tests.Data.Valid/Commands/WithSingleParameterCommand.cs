namespace Typin.Tests.Data.Valid.Commands
{
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.Common.Commands;

    [Command(nameof(WithSingleParameterCommand))]
    public class WithSingleParameterCommand : SelfSerializeCommandBase
    {
        [Parameter(0)]
        public string? ParamA { get; init; }

        public WithSingleParameterCommand(IConsole console) : base(console)
        {

        }
    }
}