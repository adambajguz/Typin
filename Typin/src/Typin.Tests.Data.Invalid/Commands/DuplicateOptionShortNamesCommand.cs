namespace Typin.Tests.Data.Invalid.Commands
{
    using Typin.Console;
    using Typin.Models.Attributes;
    using Typin.Tests.Data.Common.Commands;

    [Alias("cmd")]
    public class DuplicateOptionShortNamesCommand : SelfSerializeCommandBase
    {
        [Option('x')]
        public string? OptionA { get; init; }

        [Option('x')]
        public string? OptionB { get; init; }

        public DuplicateOptionShortNamesCommand(IConsole console) : base(console)
        {

        }
    }
}