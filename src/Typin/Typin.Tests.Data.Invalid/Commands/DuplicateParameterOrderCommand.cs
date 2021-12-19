namespace Typin.Tests.Data.Invalid.Commands
{
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.Common.Commands;

    [Command("cmd")]
    public class DuplicateParameterOrderCommand : SelfSerializeCommandBase
    {
        [Parameter(13)]
        public string? ParamA { get; init; }

        [Parameter(13)]
        public string? ParamB { get; init; }

        public DuplicateParameterOrderCommand(IConsole console) : base(console)
        {

        }
    }
}