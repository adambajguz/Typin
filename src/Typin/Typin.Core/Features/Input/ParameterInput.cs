namespace Typin.Features.Input
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Stores a parameter input.
    /// </summary>
    public sealed record ParameterInput
    {
        /// <summary>
        /// Parameter value.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Initializes an instance of <see cref="ParameterInput"/>.
        /// </summary>
        public ParameterInput(string value)
        {
            Value = value;
        }

        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            return CommandLine.EncodeArgument(Value);
        }
    }
}