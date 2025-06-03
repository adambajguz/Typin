namespace Typin.Tests.Data.Invalid.Commands
{
    using Typin.Console;
    using Typin.Models.Attributes;
    using Typin.Tests.Data.Common.Commands;

    [Alias("cmd")]
    public class SingleCharacterOptionNameCommand : SelfSerializeCommandBase
    {
        [Option("a")]
        public string? Apples { get; init; }

        public SingleCharacterOptionNameCommand(IConsole console) : base(console)
        {

        }
    }
}