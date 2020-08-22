namespace Typin.Tests.Commands.Invalid
{
    using Typin.Attributes;

    [Command("cmd")]
    public class DuplicateOptionEnvironmentVariableNamesCommand : SelfSerializeCommandBase
    {
        [CommandOption("option-a", EnvironmentVariableName = "ENV_VAR")]
        public string? OptionA { get; set; }

        [CommandOption("option-b", EnvironmentVariableName = "ENV_VAR")]
        public string? OptionB { get; set; }
    }
}