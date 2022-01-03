namespace Typin.Features.Input.Tokens
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a value token.
    /// </summary>
    public interface IToken
    {
        /// <summary>
        /// Raw values.
        /// </summary>
        IEnumerable<string> Raw { get; }
    }
}
