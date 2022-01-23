namespace Typin.Features.Input.Tokens
{
    /// <summary>
    /// Represents a directive input token.
    /// </summary>
    public interface IDirectiveToken : IBaseToken
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
        /// Whether directive was explicitly opened in command line.
        /// </summary>
        public bool IsExplicit { get; }

        /// <summary>
        /// Whether directive was terminated just after its alias.
        /// </summary>
        public bool IsTerminated { get; }

        /// <summary>
        /// Child tokens collection.
        /// </summary>
        ITokenCollection Children { get; }

        /// <summary>
        /// Whether <paramref name="alias"/> matches directive alias.
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        bool MatchesAlias(string alias);
    }
}
