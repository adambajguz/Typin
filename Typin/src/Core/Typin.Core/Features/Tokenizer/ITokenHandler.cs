namespace Typin.Features.Tokenizer
{
    using Typin.Features.Input;
    using Typin.Features.Input.Tokens;

    /// <summary>
    /// Input token handler.
    /// </summary>
    public interface ITokenHandler
    {
        /// <summary>
        /// Whether token can be handled by this handler instance.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        bool CanHandle(TokenHandlerContext context);

        /// <summary>
        /// Handles the token.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Whether token was handled.</returns>
        bool Handle(TokenHandlerContext context);
    }

    /// <summary>
    /// Input token handler.
    /// </summary>
    public interface ITokenHandler<TToken> : ITokenHandler
        where TToken : class, IToken
    {
        bool ITokenHandler.Handle(TokenHandlerContext context)
        {
            TokenGroup<TToken>? tokenGroup = context.Tokens.Get<TToken>();

            if (tokenGroup is null)
            {
                tokenGroup = new();
                context.Tokens.Add(tokenGroup);
            }

            return Handle(context, tokenGroup);
        }

        /// <summary>
        /// Handles the token of type <typeparamref name="TToken"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tokenGroup"></param>
        /// <returns>Whether token was handled.</returns>
        bool Handle(TokenHandlerContext context, TokenGroup<TToken> tokenGroup);
    }
}
