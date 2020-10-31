namespace TypinExamples.HelloWorld.Commands
{
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command]
    public class SimpleCommand : ICommand
    {
        [CommandOption("name", 'n')]
        public string? Name { get; set; }

        [CommandOption("surname", 's')]
        public string? Surname { get; set; }

        [CommandOption("mail", 'm')]
        public string? Mail { get; set; }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            if (Name == null & Surname == null)
                await console.Output.WriteLineAsync("Hello World!");
            else
                await console.Output.WriteLineAsync("Welcome " + Name + " " + Surname + " " + Mail);
        }
    }
}