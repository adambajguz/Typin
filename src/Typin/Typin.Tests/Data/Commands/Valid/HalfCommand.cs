namespace Typin.Tests.Data.Commands.Valid
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("half", Description = "Command with half type")]
    public class HalfCommand : ICommand
    {
        private readonly IConsole _console;

        [CommandOption("half")]
        public Half Value { get; init; }

        [CommandOption("half-nullable")]
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