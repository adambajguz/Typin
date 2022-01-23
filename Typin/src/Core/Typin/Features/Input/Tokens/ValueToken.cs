namespace Typin.Features.Input.Tokens
{
    using System.Collections.Generic;

    /// <summary>
    /// Stores a value.
    /// </summary>
    public sealed record ValueToken : IToken
    {
        private IEnumerable<string>? _raw;

        /// <summary>
        /// Value.
        /// </summary>
        public string Value { get; }

        /// <inheritdoc/>
        public IEnumerable<string> Raw
        {
            get
            {
                if (_raw is null)
                {
                    _raw = new string[] { Value };
                }

                return _raw;
            }
        }

        /// <summary>
        /// Initializes an instance of <see cref="ValueToken"/>.
        /// </summary>
        public ValueToken(string value)
        {
            Value = value;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Value;
        }
    }
}