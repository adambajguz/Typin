namespace Typin.Tests.Commands.Invalid
{
    using System.Collections.Generic;
    using Typin.Attributes;

    [Command("cmd")]
    public class NonLastNonScalarParameterCommand : SelfSerializeCommandBase
    {
        [CommandParameter(0)]
        public IReadOnlyList<string>? ParamA { get; set; }

        [CommandParameter(1)]
        public string? ParamB { get; set; }
    }
}