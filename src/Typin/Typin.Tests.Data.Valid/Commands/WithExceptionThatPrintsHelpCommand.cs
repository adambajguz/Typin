namespace Typin.Tests.Data.Valid.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Commands;

    [Command("cmd")]
    public class WithExceptionThatPrintsHelpCommand : ICommand
    {
        [Option("msg", 'm')]
        public string? Message { get; init; }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            throw new NullReferenceException(Message);
        }
    }
}