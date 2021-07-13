namespace Typin.Tests.Data.Commands.Invalid
{
    using System.Collections.Generic;
    using Typin.Attributes;
    using Typin.Console;
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