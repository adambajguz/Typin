namespace Typin.Features
{
    using System.Collections.Generic;
    using Typin.Features.Input;

    /// <summary>
    /// <see cref="IInputFeature"/> implementation.
    /// </summary>
    internal sealed class InputFeature : IInputFeature
    {
        /// <inheritdoc/>
        public IEnumerable<string> Arguments { get; }

        /// <inheritdoc/>
        public InputOptions Options { get; }

        /// <inheritdoc/>
        public ParsedInput? Parsed { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="InputFeature"/>.
        /// </summary>
        public InputFeature(IEnumerable<string> arguments,
                            InputOptions options)
        {
            Arguments = arguments;
            Options = options;
        }

        /// <inheritdoc/>
        public override string? ToString()
        {
            return base.ToString() +
                " | " +
                $"{nameof(Arguments)} = [\"{string.Join("\", ", Arguments)}\"], " +
                $"{nameof(Options)} = {Options}";
        }
    }
}
