namespace Typin.Tests.Data.Commands.Valid
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Attributes;

    [Command(nameof(CommandExceptionCommand))]
    public class CommandExceptionCommand : ICommand
    {
        [Option("code", 'c')]
        public int ExitCode { get; init; } = 133;

        [Option("msg", 'm')]
        public string? Message { get; init; }

        [Option("show-help")]
        public bool ShowHelp { get; init; }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            throw new NullReferenceException(Message);
        }
    }
}