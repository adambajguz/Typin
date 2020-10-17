namespace Typin.Tests.Data.Commands.Valid
{
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Exceptions;

    [Command("cmd")]
    public class CommandExceptionCommand : ICommand
    {
        [CommandOption("code", 'c')]
        public int ExitCode { get; set; } = 133;

        [CommandOption("msg", 'm')]
        public string? Message { get; set; }

        [CommandOption("show-help")]
        public bool ShowHelp { get; set; }

        public ValueTask ExecuteAsync(IConsole console)
        {
            throw new CommandException(Message, ExitCode, ShowHelp);
        }
    }
}