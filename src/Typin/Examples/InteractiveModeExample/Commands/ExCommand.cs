namespace InteractiveModeExample.Commands
{
    using System;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("ex", Description = "Throws exception that cannot be handled.")]
    public class ExCommand : ICommand
    {
        public ValueTask ExecuteAsync(IConsole console)
        {
            throw new OverflowException("Demo exception.");
        }
    }
}