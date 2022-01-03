namespace Typin.Features.Binder
{
    using System;
    using System.Collections.Generic;
    using Typin.Features.Binding;
    using Typin.Features.Input.Tokens;

    /// <summary>
    /// Stores an unbounded directive input token.
    /// </summary>
    public sealed record UnboundedDirectiveToken : IUnboundedDirectiveToken
    {
        /// <inheritdoc/>
        public bool HasUnbounded => Children?.HasUnbounded ?? false;

        /// <inheritdoc/>
        public bool IsBounded => !HasUnbounded;

        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public IUnboundedTokenCollection? Children { get; }

        /// <summary>
        /// Initializes an instance of <see cref="UnboundedDirectiveToken"/>.
        /// </summary>
        public UnboundedDirectiveToken(IDirectiveToken directiveToken)
        {
            Name = directiveToken.Name;
            Children = directiveToken.Children is null
                ? null
                : new UnboundedTokenCollection(directiveToken.Children);
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