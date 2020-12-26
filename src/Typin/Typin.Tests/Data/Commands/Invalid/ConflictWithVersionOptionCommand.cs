namespace Typin.Tests.Data.Commands.Invalid
{
    using Typin.Attributes;
    using Typin.Tests.Data.Commands;

    // Must be default because version option is available only on default commands
    [Command]
    public class ConflictWithVersionOptionCommand : SelfSerializeCommandBase
    {
        [CommandOption("version")]
        public string? Version { get; init; }
    }
}