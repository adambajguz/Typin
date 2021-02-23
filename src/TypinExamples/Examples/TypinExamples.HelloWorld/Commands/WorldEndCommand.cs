namespace TypinExamples.HelloWorld.Commands
{
    using System;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("world end", Description = "Queues the end of the World.")]
    public class WorldEndCommand : ICommand
    {
        [CommandParameter(0, Description = "The date of World's end.")]
        public DateTime Date { get; init; }

        [CommandOption("CONFIRM", IsRequired = true)]
        public bool Confirm { get; init; }

        [CommandOption("force", 'f')]
        public bool Force { get; init; }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            console.Output.Write("Queueing the end of the World");

            try
            {
                for (int i = 0; i < 5; ++i)
                {
                    await Task.Delay(350, console.GetCancellationToken());
                    console.Output.Write('.');
                }
                console.Output.WriteLine();

                if (Force || !Confirm)
                {
                    console.Output.WithForegroundColor(ConsoleColor.Red, (output) => output.Write("Sorry, I really can't do that. "));
                    console.Output.WithForegroundColor(ConsoleColor.Green, (output) => output.WriteLine("The World is safe :)"));
                }
                else
                {
                    console.Output.WithForegroundColor(ConsoleColor.Yellow, (output) => output.WriteLine("Sorry, I can't do that :<"));
                }
            }
            catch (TaskCanceledException)
            {
                console.Output.WriteLine();
                console.Output.WithForegroundColor(ConsoleColor.Green, (output) => output.WriteLine("Cancelled! The World is safe :)"));
            }
        }
    }
}