namespace Typin.Features.Binding
{
    /// <summary>
    /// Represents a directive input token.
    /// </summary>
    public interface IUnboundedDirectiveToken
    {
        /// <summary>
        /// Whether directive has unbounded directives.
        /// </summary>
        bool HasUnbounded { get; }

        /// <summary>
        /// Whether all directives tokens were bounded.
        /// </summary>
        bool IsBounded { get; }

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
        IUnboundedTokenCollection Children { get; }

        /// <summary>
        /// Whether <paramref name="name"/> matches directive alias.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool MatchesAlias(string name);
    }
}
