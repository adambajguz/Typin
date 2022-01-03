namespace Typin.Features.Binding
{
    using System;
    using System.Collections.Generic;
    using Typin.Features.Input;
    using Typin.Features.Input.Tokens;

    /// <summary>
    /// Represents a collection of unbounded tokens.
    /// </summary>
    public interface IUnboundedTokenCollection
    {
        /// <summary>
        /// Whether collection has sobe unbounded input tokens.
        /// </summary>
        bool HasUnbounded { get; }

        /// <summary>
        /// Whether all input tokens were bounded.
        /// </summary>
        bool IsBounded { get; }

        /// <summary>
        /// Builds and returns a raw input.
        /// </summary>
        /// <returns></returns>
        IList<string> GetRaw();

        /// <summary>
        /// Gets a given input group.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>The requested input group, or null if it is not present.</returns>
        ITokenGroup? this[Type key] { get; }

        /// <summary>
        /// Retrieves the requested input group from the collection.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>The requested input group, or null if it is not present.</returns>
        ITokenGroup? Get(Type key);

        /// <summary>
        /// Retrieves the requested input group from the collection.
        /// </summary>
        /// <typeparam name="TInputToken"></typeparam>
        /// <returns>The requested input group, or null if it is not present.</returns>
        TokenGroup<TInputToken>? Get<TInputToken>()
            where TInputToken : class, IToken;

        /// <summary>
        /// Removes a input group in the collection by key.
        /// </summary>
        /// <param name="key">The input group key.</param>
        bool Remove(Type key);
    }
}
