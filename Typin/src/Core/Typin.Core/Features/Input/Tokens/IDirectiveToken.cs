namespace Typin.Features.Input.Tokens
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a directive input token.
    /// </summary>
    public interface IDirectiveToken
    {
        /// <summary>
        /// Directive name (may be <see cref="string.Empty"/>).
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Child tokens collection.
        /// </summary>
        ITokenCollection? Children { get; set; }

        /// <summary>
        /// Raw values.
        /// </summary>
        IEnumerable<string> Raw { get; }

        /// <summary>
        /// Whether <paramref name="name"/> matches directive name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool MatchesName(string name);
    }
}
