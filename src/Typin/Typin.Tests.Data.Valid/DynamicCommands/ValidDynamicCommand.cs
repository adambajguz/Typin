namespace Typin.Tests.Data.DynamicCommands.Valid
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.DynamicCommands;

    public class ValidDynamicCommand : IDynamicCommand
    {
        private readonly IConsole _console;

        [Parameter(0)]
        public string Param0 { get; init; } = string.Empty;

        [Option("opt0")]
        public string Opt0 { get; init; } = string.Empty;

        [Option("opt1")]
        public int Opt1 { get; init; }

        public IArgumentCollection Arguments { get; init; } = default!;

        public ValidDynamicCommand(IConsole console)
        {
            _console = console;
        }

        [SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value", Justification = "For debugging.")]
        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            _console.Output.WriteLine(JsonConvert.SerializeObject(this, Formatting.None));

            InputValue? numberInput = Arguments.TryGet("Number");

            int number0 = Arguments["Number"].GetValueOrDefault<int>();
            int number1 = Arguments["Opt4"].GetValueOrDefault<int>();
            int? number2 = Arguments["Opt5"].GetValueOrDefault<int?>();
            double number3 = Arguments["Price"].GetValueOrDefault<double>();

            string? param0 = Arguments.Get("Parameter").GetValueOrDefault<string?>();
            string? param2 = Arguments.Get("Param2").GetValueOrDefault<string>();
            string? param1 = Arguments.Get("Str").GetValueOrDefault<string>();

            return default;
        }
    }
}
