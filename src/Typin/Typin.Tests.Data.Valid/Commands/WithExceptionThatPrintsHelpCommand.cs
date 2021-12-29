namespace Typin.Tests.Data.Valid.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Commands;
    using Typin.Commands.Attributes;
    using Typin.Models.Attributes;

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