namespace Typin.Tests.Data.Invalid.Commands
{
    using System.Collections.Generic;
    using Typin.Commands.Attributes;
    using Typin.Console;
    using Typin.Models.Attributes;
    using Typin.Tests.Data.Common.Commands;

    [Command("cmd")]
    public class NonLastNonScalarParameterCommand : SelfSerializeCommandBase
    {
        [Parameter(0)]
        public IReadOnlyList<string>? ParamA { get; init; }

        [Parameter(1)]
        public string? ParamB { get; init; }

        public NonLastNonScalarParameterCommand(IConsole console) : base(console)
        {

        }
    }
}