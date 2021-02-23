namespace TypinExamples.Validation.Commands
{
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("email", Description = "Email validation example.")]
    public class EmailCommand : ICommand
    {
        [CommandOption("address", 'a', IsRequired = true)]
        public string? Email { get; set; }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await console.Output.WriteLineAsync(Email);
        }
    }
}