namespace Typin.Features.Input.Tokens
{
    using System.Collections.Generic;

    /// <summary>
    /// Stores named values.
    /// </summary>
    public sealed record NamedToken : IToken
    {
        private IEnumerable<string>? _raw;

        /// <summary>
        /// Option alias.
        /// </summary>
        public string Alias { get; }

        /// <summary>
        /// Option values.
        /// </summary>
        public IReadOnlyList<string> Values { get; }

        /// <inheritdoc/>
        public IEnumerable<string> Raw
        {
            get
            {
                if (_raw is null)
                {
                    List<string> values = new(Values.Count + 1)
                    {
                        GetFormattedAlias()
                    };
                    values.AddRange(Values);

                    _raw = values;
                }

                return _raw;
            }
        }

        /// <summary>
        /// Initializes an instance of <see cref="NamedToken"/>.
        /// </summary>
        public NamedToken(string alias)
        {
            Alias = alias.Trim().TrimStart('-');
            Values = new List<string>();
        }

        /// <summary>
        /// Initializes an instance of <see cref="NamedToken"/>.
        /// </summary>
        public NamedToken(string alias, IReadOnlyList<string> values)
        {
            Alias = alias.Trim().TrimStart('-');
            Values = values;
        }

        /// <summary>
        /// Get formatted <see cref="Alias"/>.
        /// </summary>
        /// <returns></returns>
        public string GetFormattedAlias()
        {
            return Alias switch
            {
                { Length: 0 } => Alias,
                { Length: 1 } => $"-{Alias}",
                _ => $"--{Alias}"
            };
        }
        /// <inheritdoc/>
        public override string ToString()
        {
            if (Values.Count == 0)
            {
                return GetFormattedAlias();
            }

            return string.Concat(
                GetFormattedAlias(),
                " {",
                string.Join(", ", Values),
                "}");
        }
    }
}