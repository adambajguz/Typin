namespace Typin.Tests.Data.Invalid.DuplicatedCommands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Commands;

    [Command("duplicated-ex")]
    public class GenericExceptionCommand : ICommand
    {
        [Option("msg", 'm')]
        public string? Message { get; init; }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            throw new Exception(Message);
        }
    }
}