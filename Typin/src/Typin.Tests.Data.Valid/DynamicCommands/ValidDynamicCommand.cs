namespace Typin.Tests.Data.Valid.DynamicCommands
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Typin.Commands;
    using Typin.Console;
    using Typin.Models;
    using Typin.Models.Attributes;
    using Typin.Models.Collections;

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

            ArgumentValue? numberInput = Arguments.GetOrDefault("Number");

            int number0 = Arguments["Number"].GetValue<int>(0);
            int number1 = Arguments["Opt4"].GetValue<int>(0);
            int? number2 = Arguments["Opt5"].GetValue<int?>(null);
            double number3 = Arguments["Price"].GetValue<double>(0);

            string? param0 = Arguments.Get("Parameter").GetValue<string?>();
            string? param2 = Arguments.Get("Param2").GetValue<string>();
            string? param1 = Arguments.Get("Str").GetValue<string>();

            return default;
        }
    }
}
