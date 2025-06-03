namespace Typin.Tests.Data.Invalid.Commands
{
    using Typin.Commands.Attributes;
    using Typin.Console;
    using Typin.Models.Attributes;
    using Typin.Schemas.Attributes;
    using Typin.Tests.Data.Common.Commands;

    [Alias("cmd")]
    public class DuplicateOptionNamesCommand : SelfSerializeCommandBase
    {
        [Option("fruits")]
        public string? Apples { get; init; }

        [Option("fruits")]
        public string? Oranges { get; init; }

        public DuplicateOptionNamesCommand(IConsole console) : base(console)
        {

        }
    }
}