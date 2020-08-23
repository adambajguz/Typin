namespace Typin.Tests.Data.Commands.Valid
{
    using System.Collections.Generic;
    using Typin.Attributes;
    using Typin.Tests.Data.Commands;

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