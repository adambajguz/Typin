namespace Typin.Tests.Data.Valid.Commands
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Commands;
    using Typin.Console;

    [Command("half", Description = "Command with half type")]
    public class HalfCommand : ICommand
    {
        private readonly IConsole _console;

        [Option("half")]
        public Half Value { get; init; }

        [Option("half-nullable")]
        public Half? NullableValue { get; init; }

        public HalfCommand(IConsole console)
        {
            _console = console;
        }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            _console.Output.WriteLine($"Value:{Value.ToString(CultureInfo.InvariantCulture)}");
            _console.Output.WriteLine($"NullableValue:{(NullableValue.HasValue ? NullableValue.Value.ToString(CultureInfo.InvariantCulture) : "null")}");

            return default;
        }
    }
}