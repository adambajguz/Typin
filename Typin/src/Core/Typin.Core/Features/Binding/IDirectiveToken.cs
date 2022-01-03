namespace Typin.Features.Binding
{
    using System.Collections.Generic;

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
        /// Directive name (may be <see cref="string.Empty"/>).
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Child tokens collection.
        /// </summary>
        IUnboundedTokenCollection? Children { get; }

        /// <summary>
        /// Whether <paramref name="name"/> matches directive name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool MatchesName(string name);
    }
}
