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
        public IEnumerable<string> Original { get; }

        /// <inheritdoc/>
        public InputOptions Options { get; }

        /// <inheritdoc/>
        public IDirectiveCollection? Tokens { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="InputFeature"/>.
        /// </summary>
        public InputFeature(IEnumerable<string> arguments,
                            InputOptions options)
        {
            Original = arguments;
            Options = options;
        }

        /// <inheritdoc/>
        public override string? ToString()
        {
            return base.ToString() +
                " | " +
                $"{nameof(Original)} = [{string.Join(';', Original)}], " +
                $"{nameof(Options)} = {{{Options}}}, " +
                $"{nameof(Tokens)} = {{{Tokens}}}";
        }
    }
}
