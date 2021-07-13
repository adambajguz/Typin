namespace Typin.Tests.Data.Commands.Valid
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Exceptions;

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
            throw new CommandException(Message, ExitCode, ShowHelp);
        }
    }
}