namespace Typin.Tests.Data.Commands.Invalid
{
    using Typin.Attributes;
    using Typin.Tests.Data.Commands;

    [Command("cmd")]
    public class DuplicateOptionEnvironmentVariableNamesCommand : SelfSerializeCommandBase
    {
        [CommandOption("option-a", EnvironmentVariableName = "ENV_VAR")]
        public string? OptionA { get; set; }

        [CommandOption("option-b", EnvironmentVariableName = "ENV_VAR")]
        public string? OptionB { get; set; }
    }
}