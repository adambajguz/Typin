namespace InteractiveModeExample.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command(Description = "Default Command Description")]
    public class DefaultCommand : ICommand
    {
        [CommandParameter(0)]
        public IReadOnlyList<string> Values { get; set; } = default!;

        public DefaultCommand()
        {

        }

        public ValueTask ExecuteAsync(IConsole console)
        {
            console.Output.WithForegroundColor(ConsoleColor.DarkGreen, (output) => output.WriteLine("Hello world from default command"));

            foreach (var value in Values)
                console.Output.WriteLine(value);

            return default;
        }
    }
}