namespace Typin.Features.Input.Tokens
{
    using System;
    using System.Collections.Generic;
    using Typin.Features.Input;

    /// <summary>
    /// Stores a directive input token.
    /// </summary>
    public sealed record DirectiveToken : IDirectiveToken
    {
        /// <inheritdoc/>
        public int Id { get; }

        /// <inheritdoc/>
        public string Alias { get; }

        /// <inheritdoc/>
        public bool IsExplicit { get; }

        /// <inheritdoc/>
        public bool IsTerminated { get; }

        /// <inheritdoc/>
        public ITokenCollection Children { get; }

        /// <inheritdoc/>
        public IEnumerable<string> Raw
        {
            get
            {
                List<string> values = new();

                if (Children is { Count: > 0 })
                {
                    values.Add(string.Concat("[", Alias, ": "));
                    values.AddRange(Children.GetRaw());

                    values[^1] += "]";
                }
                else
                {
                    values.Add(string.Concat("[", Alias, "]"));
                }

                return values;
            }
        }

        /// <summary>
        /// Initializes an instance of <see cref="DirectiveToken"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public DirectiveToken(int id, string name) :
            this(id,
                 name,
                 name.StartsWith('['),
                 name.EndsWith(']'))
        {

        }

        /// <summary>
        /// Initializes an instance of <see cref="DirectiveToken"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="isExplicit"></param>
        /// <param name="isTermianted"></param>
        public DirectiveToken(int id, string name, bool isExplicit, bool isTermianted)
        {
            Id = id;
            Alias = name.Trim('[', ']')
                .Trim()
                .TrimEnd(':')
                .TrimEnd();

            IsExplicit = isExplicit;
            IsTerminated = isTermianted;
            Children = new TokenCollection();
        }

        /// <inheritdoc/>
        public bool MatchesAlias(string name)
        {
            return name.Trim('[', ']')
                .Trim()
                .TrimEnd(':')
                .TrimEnd()
                .Equals(Alias, StringComparison.Ordinal);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (Children is null)
            {
                return $"[{Alias}]";
            }

            string v = string.Join(", ", Children.GetRaw());

            return $"[{Alias}: {{{v}}}]";
        }
    }
}