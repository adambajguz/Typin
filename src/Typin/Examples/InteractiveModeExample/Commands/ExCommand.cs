namespace InteractiveModeExample.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Commands;

    [Command("ex", Description = "Throws exception that cannot be handled.")]
    public class ExCommand : ICommand
    {
        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            throw new OverflowException("Demo exception.");
        }
    }
}