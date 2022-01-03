namespace Typin.Features.Input
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Typin.Features.Input.Tokens;

    /// <summary>
    /// Represents a value token group with tokens of type <typeparamref name="T"/>.
    /// </summary>
    public sealed class TokenGroup<T> : ITokenGroup
        where T : class, IToken
    {
        private readonly List<T> _tokens;

        /// <inheritdoc/>
        public Type TokenType { get; }

        /// <summary>
        /// Input tokens collection.
        /// </summary>
        public IList<T> Tokens => _tokens;

        IReadOnlyList<IToken> ITokenGroup.Tokens => _tokens;

        /// <summary>
        /// Initializes a new instanc of <see cref="TokenGroup{T}"/>.
        /// </summary>
        public TokenGroup() :
            this(new List<T>())
        {

        }

        /// <summary>
        /// Initializes a new instanc of <see cref="TokenGroup{T}"/>.
        /// </summary>
        public TokenGroup(List<T> tokens)
        {
            _tokens = tokens;
            TokenType = typeof(T);
        }

        /// <summary>
        /// Builds and returns raw tokens.
        /// </summary>
        /// <returns></returns>
        public IList<string> GetRaw()
        {
            List<string> tmp = new();

            foreach (T token in _tokens)
            {
                tmp.AddRange(token.Raw);
            }

            return tmp;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return base.ToString() +
                " | " +
                $"{nameof(TokenType)} = {TokenType}";
        }

        ITokenGroup ITokenGroup.DeepClone()
        {
            return DeepClone();
        }

        /// <inheritdoc/>
        public TokenGroup<T> DeepClone()
        {
            return new TokenGroup<T>(Tokens.ToList());
        }
    }
}
