namespace Typin.Tests.Data.Commands.Valid
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Attributes;

    [Command("cmd")]
    public class GenericExceptionCommand : ICommand
    {
        [CommandOption("msg", 'm')]
        public string? Message { get; init; }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            throw new Exception(Message);
        }
    }
}