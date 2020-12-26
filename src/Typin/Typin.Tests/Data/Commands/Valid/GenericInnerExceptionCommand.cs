namespace Typin.Tests.Data.Commands.Valid
{
    using System;
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Console;

    [Command("cmd")]
    public class GenericInnerExceptionCommand : ICommand
    {
        [CommandOption("msg", 'm')]
        public string? Message { get; init; }

        [CommandOption("inner-msg", 'i')]
        public string? InnerMessage { get; init; }

        public ValueTask ExecuteAsync(IConsole console)
        {
            throw new Exception(Message, new Exception(InnerMessage));
        }
    }
}
