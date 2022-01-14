namespace Typin.Features.Binder
{
    using System;
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
        public int Id { get; }

        /// <inheritdoc/>
        public string Alias { get; }

        /// <inheritdoc/>
        public IUnboundedTokenCollection? Children { get; }

        /// <summary>
        /// Initializes an instance of <see cref="UnboundedDirectiveToken"/>.
        /// </summary>
        public UnboundedDirectiveToken(IDirectiveToken directiveToken)
        {
            Id = directiveToken.Id;
            Alias = directiveToken.Alias;
            Children = directiveToken.Children is null
                ? null
                : new UnboundedTokenCollection(directiveToken.Children);
        }

        /// <inheritdoc/>
        public bool MatchesAlias(string alias)
        {
            return alias.Trim('[', ']')
                .Trim()
                .TrimEnd(':')
                .Equals(Alias, StringComparison.Ordinal);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return base.ToString() +
                " | " +
                $"{nameof(Id)} = {Id}, " +
                $"{nameof(Alias)} = {Alias}";
        }
    }
}