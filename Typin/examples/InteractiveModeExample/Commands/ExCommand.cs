namespace InteractiveModeExample.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Commands;
    using Typin.Schemas.Attributes;

    [Alias("ex")]
    public class ExCommand : ICommand
    {
        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            throw new OverflowException("Demo exception.");
        }
    }
}