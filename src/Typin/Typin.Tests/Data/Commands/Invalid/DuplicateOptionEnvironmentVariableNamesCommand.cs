namespace Typin.Tests.Data.Commands.Invalid
{
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.Commands;

    [Command("cmd")]
    public class DuplicateOptionEnvironmentVariableNamesCommand : SelfSerializeCommandBase
    {
        [CommandOption("option-a", FallbackVariableName = "ENV_VAR")]
        public string? OptionA { get; init; }

        [CommandOption("option-b", FallbackVariableName = "ENV_VAR")]
        public string? OptionB { get; init; }

        public DuplicateOptionEnvironmentVariableNamesCommand(IConsole console) : base(console)
        {

        }
    }
}