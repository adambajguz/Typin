namespace Typin.Tests.Commands.Valid
{
    using System.Collections.Generic;
    using Typin.Attributes;

    [Command("cmd")]
    public class WithParametersCommand : SelfSerializeCommandBase
    {
        [CommandParameter(0)]
        public string? ParamA { get; set; }

        [CommandParameter(1)]
        public int? ParamB { get; set; }

        [CommandParameter(2)]
        public IReadOnlyList<string>? ParamC { get; set; }
    }
}