namespace Typin.Features
{
    using System.Collections.Generic;

    /// <summary>
    /// <see cref="IInputFeature"/> implementation.
    /// </summary>
    public sealed class InputFeature : IInputFeature
    {
        /// <inheritdoc/>
        public IEnumerable<string> Arguments { get; }

        /// <inheritdoc/>
        public InputOptions Options { get; }

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
                $"{nameof(Arguments)} = [{string.Join(';', Arguments)}], " +
                $"{nameof(Options)} = {{{Options}}}";
        }
    }
}
