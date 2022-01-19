namespace InteractiveModeExample.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Commands;
    using Typin.Console;
    using Typin.Models.Attributes;
    using Typin.Schemas.Attributes;

    [Alias]
    public class DefaultCommand : ICommand
    {
        private readonly IConsole _console;

        [Parameter(0)]
        public IReadOnlyList<string> Values { get; init; } = default!;

        public DefaultCommand(IConsole console)
        {
            _console = console;
        }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            _console.Output.WithForegroundColor(ConsoleColor.DarkGreen, (output) => output.WriteLine("Hello world from default command"));

            foreach (var value in Values)
            {
                _console.Output.WriteLine(value);
            }

            return default;
        }
    }
}