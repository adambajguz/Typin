namespace Typin.Tests.Data.Commands.Valid
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Attributes;

    [Command]
    public class WithNullableEnumCollectionArgumentsCommand : ICommand
    {
        public enum CustomEnum1 { Value1, Value2, Value3 };
        public enum CustomEnum2 { Value4, Value5, Value6 };
        public enum CustomEnum3 { Value7, Value8, Value9 };
        public enum CustomEnum4 { ValueA, ValueB, ValueC };
        public enum CustomEnum5 { ValueD, ValueE, ValueF };

        [CommandParameter(0, Name = "Foo")]
        public List<CustomEnum1?> Foo { get; set; } = default!;

        [CommandOption("bar")]
        public IList<CustomEnum2?> Bar { get; set; } = default!;

        [CommandOption("wizz")]
        public IEnumerable<CustomEnum3?> Wizz { get; set; } = default!;

        [CommandOption("Buzz")]
        public CustomEnum4?[] Buzz { get; set; } = default!;

        [CommandOption("fizzz")]
        public IReadOnlyCollection<CustomEnum5?> Fizz { get; set; } = new List<CustomEnum5?>() { CustomEnum5.ValueD, CustomEnum5.ValueF, null };

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            return default;
        }
    }
}