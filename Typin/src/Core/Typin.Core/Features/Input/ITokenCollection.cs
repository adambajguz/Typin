namespace Typin.Features.Input
{
    using System;
    using System.Collections.Generic;
    using Typin.Features.Input.Tokens;

    /// <summary>
    /// Represents a collection of tokens.
    /// </summary>
    public interface ITokenCollection : IEnumerable<KeyValuePair<Type, ITokenGroup>>
    {
        /// <summary>
        /// Tokens count.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Builds and returns a raw input.
        /// </summary>
        /// <returns></returns>
        IList<string> GetRaw();

        /// <summary>
        /// Gets or sets a given tokens group. Setting a null value removes the tokens group.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>The requested tokens group, or null if it is not present.</returns>
        ITokenGroup? this[Type key] { get; set; }

        /// <summary>
        /// Retrieves the requested tokens group from the collection.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>The requested tokens group, or null if it is not present.</returns>
        ITokenGroup? Get(Type key);

        /// <summary>
        /// Retrieves the requested tokens group from the collection.
        /// </summary>
        /// <typeparam name="TToken"></typeparam>
        /// <returns>The requested tokens group, or null if it is not present.</returns>
        TokenGroup<TToken>? Get<TToken>()
            where TToken : class, IToken;

        /// <summary>
        /// Sets the given tokens group in the collection.
        /// </summary>
        /// <param name="instance">The tokens group value.</param>
        void Add(ITokenGroup instance);

        /// <summary>
        /// Removes a tokens group in the collection by key.
        /// </summary>
        /// <param name="key">The tokens group key.</param>
        bool Remove(Type key);
    }
}
