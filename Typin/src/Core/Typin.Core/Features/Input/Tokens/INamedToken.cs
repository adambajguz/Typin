namespace Typin.Features.Input.Tokens
{
    using System.Collections.Generic;

    /// <summary>
    /// Named token.
    /// </summary>
    public interface INamedToken : IToken
    {
        /// <summary>
        /// Option alias.
        /// </summary>
        string Alias { get; }

        /// <summary>
        /// Option values.
        /// </summary>
        IReadOnlyList<string> Values { get; }

        /// <summary>
        /// Get formatted <see cref="Alias"/>.
        /// </summary>
        /// <returns></returns>
        string GetFormattedAlias();
    }
}