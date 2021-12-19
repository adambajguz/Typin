namespace Typin.Tests.Data.Invalid.Commands
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