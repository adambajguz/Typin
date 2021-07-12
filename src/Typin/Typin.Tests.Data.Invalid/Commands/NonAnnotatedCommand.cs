namespace Typin.Tests.Data.Commands.Invalid
{
    using Typin.Console;
    using Typin.Tests.Data.Common.Commands;

    public class NonAnnotatedCommand : SelfSerializeCommandBase
    {
        public NonAnnotatedCommand(IConsole console) : base(console)
        {

        }
    }
}