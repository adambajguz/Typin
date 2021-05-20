namespace Typin.Tests.Data.Commands.Valid
{
    using System.Globalization;
    using System;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("half", Description = "Command with half type")]
    public class HalfCommand : ICommand
    {
        [CommandOption("half")]
        public Half Value { get; init; }

        [CommandOption("half-nullable")]
        public Half? NullableValue { get; init; }

        public ValueTask ExecuteAsync(IConsole console)
        {
            console.Output.WriteLine($"Value:{Value.ToString(CultureInfo.InvariantCulture)}");
            console.Output.WriteLine($"NullableValue:{(NullableValue.HasValue ? NullableValue.Value.ToString(CultureInfo.InvariantCulture) : "null")}");

            return default;
        }
    }
}