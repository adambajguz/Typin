namespace Typin.Tests.Data.Commands.Valid
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Attributes;

    [Command("duplicated-ex")]
    public class GenericInnerExceptionCommand : ICommand
    {
        [Option("msg", 'm')]
        public string? Message { get; init; }

        [Option("inner-msg", 'i')]
        public string? InnerMessage { get; init; }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            throw new Exception(Message, new Exception(InnerMessage));
        }
    }
}
