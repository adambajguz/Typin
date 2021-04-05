namespace Typin.Tests.Data.Commands.Valid
{
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Exceptions;

    [Command("cmd")]
    public class WithExceptionThatPrintsHelpCommand : ICommand
    {
        [CommandOption("msg", 'm')]
        public string? Message { get; init; }

        public ValueTask ExecuteAsync(IConsole console)
        {
            throw new CommandException(Message, showHelp: true);
        }
    }
}