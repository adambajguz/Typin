namespace Typin.Tests.Data.Invalid.Commands
{
    using System.Collections.Generic;
    using Typin.Commands.Attributes;
    using Typin.Console;
    using Typin.Models.Attributes;
    using Typin.Tests.Data.Common.Commands;

    [Command("cmd")]
    public class MultipleNonScalarParametersCommand : SelfSerializeCommandBase
    {
        [Parameter(0)]
        public IReadOnlyList<string>? ParamA { get; init; }

        [Parameter(1)]
        public IReadOnlyList<string>? ParamB { get; init; }

        public MultipleNonScalarParametersCommand(IConsole console) : base(console)
        {

        }
    }
}