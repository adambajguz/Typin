namespace Typin.Tests.Data.Commands.Invalid
{
    using System.Collections.Generic;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.Commands;

    [Command("cmd")]
    public class NonLastNonScalarParameterCommand : SelfSerializeCommandBase
    {
        [CommandParameter(0)]
        public IReadOnlyList<string>? ParamA { get; init; }

        [CommandParameter(1)]
        public string? ParamB { get; init; }

        public NonLastNonScalarParameterCommand(IConsole console) : base(console)
        {

        }
    }
}