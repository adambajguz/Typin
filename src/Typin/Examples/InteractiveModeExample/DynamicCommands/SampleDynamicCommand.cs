namespace InteractiveModeExample.DynamicCommands
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

            InputValue? numberInput = Arguments.GetOrDefault("Number");
            int? number = Arguments["Number"]?.GetValue<int?>();
            int number2 = Arguments["Number"].GetValue<int>(0);
            int? number5 = Arguments["Opt5"]?.GetValue<int?>();
            string? param0 = Arguments.Get("Str").GetValue<string?>();
            string? param1 = Arguments.Get("Str").GetValue<string>();
            string param2 = Arguments.Get("Str").GetValue<string>("str");

            var filteredArgs = Arguments
                .Where(x => x.Value.Metadata.Get<ArgumentMetadata>() is ArgumentMetadata a && a.Tags.Contains("test"))
                .ToList();

            return default;
        }
    }
}
