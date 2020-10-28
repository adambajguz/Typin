namespace TypinExamples.HelloWorld.Commands
{
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command]
    public class WelcomeCommand : ICommand
    {
        [CommandOption("name", 'n')]
        public string Name { get; set; }

        [CommandOption("surname", 's')]
        public string Surname { get; set; }

        public ValueTask ExecuteAsync(IConsole console)
        {
            return default;
        }
    }
}