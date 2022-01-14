namespace Typin.Features.Input.Tokens
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a directive input token.
    /// </summary>
    public interface IDirectiveToken
    {
        /// <summary>
        /// Directive identifier.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Directive alias (may be <see cref="string.Empty"/>).
        /// </summary>
        string Alias { get; }

        /// <summary>
        /// Child tokens collection.
        /// </summary>
        ITokenCollection Children { get; }

        /// <summary>
        /// Raw values.
        /// </summary>
        IEnumerable<string> Raw { get; }

        /// <summary>
        /// Whether <paramref name="alias"/> matches directive alias.
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        bool MatchesAlias(string alias);
    }
}
