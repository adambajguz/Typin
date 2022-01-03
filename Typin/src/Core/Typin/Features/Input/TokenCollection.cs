namespace Typin.Features.Input
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Typin.Features.Input.Tokens;

    /// <summary>
    /// Default implementation of <see cref="ITokenCollection"/>.
    /// </summary>
    public class TokenCollection : ITokenCollection
    {
        /// <summary>
        /// Data.
        /// </summary>
        protected Dictionary<Type, ITokenGroup> Data { get; set; } = new();

        /// <inheritdoc/>
        public int Count => Data.Count;

        /// <summary>
        /// Initializes a new instance of <see cref="TokenCollection"/>.
        /// </summary>
        public TokenCollection()
        {

        }

        /// <inheritdoc />
        public ITokenGroup? this[Type key]
        {
            get
            {
                _ = key ?? throw new ArgumentNullException(nameof(key));

                return Data.GetValueOrDefault(key);
            }
            set
            {
                _ = key ?? throw new ArgumentNullException(nameof(key));

                if (value is null)
                {
                    Data.Remove(key);

                    return;
                }

                Data[key] = value;
            }
        }

        /// <inheritdoc />
        public ITokenGroup? Get(Type key)
        {
            return this[key];
        }

        /// <inheritdoc />
        public TokenGroup<TToken>? Get<TToken>()
            where TToken : class, IToken
        {
            return this[typeof(TToken)] as TokenGroup<TToken>;
        }

        /// <inheritdoc />
        public void Set(ITokenGroup instance)
        {
            this[instance.TokenType] = instance;
        }

        /// <inheritdoc />
        public bool Remove(Type key)
        {
            _ = key ?? throw new ArgumentNullException(nameof(key));

            return Data.Remove(key);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<Type, ITokenGroup>> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        /// <inheritdoc/>
        public IList<string> GetRaw()
        {
            List<string> tmp = new();

            foreach (KeyValuePair<Type, ITokenGroup> tokenGroup in Data)
            {
                tmp.AddRange(tokenGroup.Value.GetRaw());
            }

            return tmp;
        }
    }
}