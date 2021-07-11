namespace InteractiveModeExample.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.DynamicCommands;

    public class SampleDynamicCommand : IDynamicCommand
    {
        private readonly IConsole _console;

        [Option("author", 'a')]
        public string Author { get; init; } = string.Empty;

        [Option("date", 'd')]
        public DateTime Date { get; init; } = DateTime.Now;

        public IDynamicArgumentCollection Arguments { get; init; } = default!;

        public SampleDynamicCommand(IConsole console)
        {
            _console = console;
        }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            //add dynamic --name abc
            //abc --help
            //abc

            var number = Arguments.GetValueOrDefault("Number");

            _console.Output.WriteLine(System.Text.Json.JsonSerializer.Serialize(this));

            return default;
        }
    }
}
