namespace Typin.Tests.Data.Common.Help
{
    using System.Collections.Generic;
    using Typin.Commands.Schemas;
    using Typin.Console;
    using Typin.Help;
    using Typin.Models.Schemas;

    public class TestHelpWriter : IHelpWriter
    {
        private readonly IConsole _console;

        public const string ExpectedStringOnExceptionWrite = "ExceptionW";
        public const string ExpectedStringOnStandardWrite = "CommandsWStandard";

        /// <summary>
        /// Initializes an instance of <see cref="TestHelpWriter"/>.
        /// </summary>
        public TestHelpWriter(IConsole console)
        {
            _console = console;
        }

        /// <inheritdoc/>
        public void Write()
        {
            _console.Output.WriteLine(ExpectedStringOnExceptionWrite);
        }

        /// <inheritdoc/>
        public void Write(ICommandSchema command,
                          IReadOnlyDictionary<IArgumentSchema, object?> defaultValues)
        {
            _console.Output.WriteLine(ExpectedStringOnStandardWrite);
        }
    }
}