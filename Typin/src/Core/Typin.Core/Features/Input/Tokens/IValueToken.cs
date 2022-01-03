namespace Typin.Features.Input.Tokens
{
    /// <summary>
    /// Value token.
    /// </summary>
    public interface IValueToken : IToken
    {
        /// <summary>
        /// Value.
        /// </summary>
        string Value { get; }
    }
}