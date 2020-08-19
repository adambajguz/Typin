using System.Diagnostics.CodeAnalysis;

namespace Typin.Input
{
    /// <summary>
    /// Stores command parameter input.
    /// </summary>
    public class CommandParameterInput
    {
        /// <summary>
        /// Parameter value.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Initializes an instance of <see cref="CommandParameterInput"/>.
        /// </summary>
        public CommandParameterInput(string value)
        {
            Value = value;
        }

        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            return Value;
        }
    }
}