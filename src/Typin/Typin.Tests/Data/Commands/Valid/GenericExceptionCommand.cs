namespace Typin.Tests.Data.Commands.Valid
{
    using System;
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Console;

    [Command("cmd")]
    public class GenericExceptionCommand : ICommand
    {
        [CommandOption("msg", 'm')]
        public string? Message { get; set; }

        public ValueTask ExecuteAsync(IConsole console)
        {
            throw new Exception(Message);
        }
    }
}