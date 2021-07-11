namespace InteractiveModeExample.Commands
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.DynamicCommands;
    using Typin.Metadata;

    public class SampleDynamicCommand : IDynamicCommand
    {
        private readonly IConsole _console;

        [Option("author", 'a')]
        public string Author { get; init; } = string.Empty;

        [Option("date", 'd')]
        public DateTime Date { get; init; } = DateTime.Now;

        public IArgumentCollection Arguments { get; init; } = default!;

        public SampleDynamicCommand(IConsole console)
        {
            _console = console;
        }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            _console.Output.WriteLine(JsonConvert.SerializeObject(this, Formatting.Indented));

            InputValue? numberInput = Arguments.TryGet("Number");
            int? number = Arguments["Number"].GetValueOrDefault<int?>();
            int number2 = Arguments["Number"].GetValueOrDefault<int>();
            int? number5 = Arguments["Opt5"].GetValueOrDefault<int?>();
            string? param0 = Arguments.Get("Str").GetValueOrDefault<string?>();
            string? param1 = Arguments.Get("Str").GetValueOrDefault<string>();

            var filteredArgs = Arguments
                .Where(x => x.Value.Metadata.GetValueOrDefault<ArgumentMetadata>() is ArgumentMetadata a && a.Tags.Contains("test"))
                .ToList();

            return default;
        }
    }
}
