namespace Typin.Features.Input.Tokens
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Typin.Features.Input;

    /// <summary>
    /// Stores a directive input token.
    /// </summary>
    public sealed record DirectiveToken : IDirectiveToken
    {
        private IEnumerable<string>? _raw;

        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public ITokenCollection? Children { get; set; }

        /// <inheritdoc/>
        public IEnumerable<string> Raw
        {
            get
            {
                if (_raw is null)
                {
                    List<string> values = new();

                    values.Add(string.Concat("[", Name, ": "));

                    if (Children is not null)
                    {
                        values.AddRange(Children.GetRaw());
                    }

                    values[^1] += "]";

                    _raw = values;
                }

                return _raw;
            }
        }

        /// <summary>
        /// Initializes an instance of <see cref="DirectiveToken"/>.
        /// </summary>
        public DirectiveToken(string name)
        {
            Name = name.Trim('[', ']')
                .Trim()
                .TrimEnd(':');
        }

        /// <inheritdoc/>
        public bool MatchesName(string name)
        {
            return name.Trim('[', ']')
                .Trim()
                .TrimEnd(':')
                .Equals(Name, StringComparison.Ordinal);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return string.Concat("[", Name, "]");
        }
    }
}