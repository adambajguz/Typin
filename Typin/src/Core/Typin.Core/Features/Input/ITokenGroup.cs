namespace Typin.Features.Input
{
    using System;
    using System.Collections.Generic;
    using Typin.Features.Input.Tokens;

    /// <summary>
    /// Represents a value token group.
    /// </summary>
    public interface ITokenGroup
    {
        /// <summary>
        /// Token type.
        /// </summary>
        Type TokenType { get; }

        /// <summary>
        /// Input tokens collection.
        /// </summary>
        IReadOnlyList<IToken> Tokens { get; }

        /// <summary>
        /// Builds and returns raw tokens.
        /// </summary>
        /// <returns></returns>
        IList<string> GetRaw();

        /// <summary>
        /// Deep clones current token group.
        /// </summary>
        /// <returns></returns>
        ITokenGroup DeepClone();
    }
}
