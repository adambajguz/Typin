namespace Typin.Features.Tokenizer.Handlers
{
    using Typin.Features.Input;
    using Typin.Features.Input.Tokens;

    /// <summary>
    /// <see cref="ValueToken"/> handler.
    /// </summary>
    public sealed class ValueTokenHandler : ITokenHandler<ValueToken>
    {
        /// <inheritdoc/>
        public bool CanHandle(TokenHandlerContext context)
        {
            return context.Position < context.Arguments.Count;
        }

        /// <inheritdoc/>
        public bool Handle(TokenHandlerContext context, TokenGroup<ValueToken> tokenGroup)
        {
            if (context.Position >= context.Arguments.Count)
            {
                return false;
            }

            string argument = context.Arguments[context.Position++];

            bool hasDirectiveTermination = context.Directive.IsExplicit && argument.EndsWith(']');

            argument = hasDirectiveTermination
                ? argument[..^1]
                : argument;

            tokenGroup.Tokens.Add(new ValueToken(argument));

            return true;
        }
    }
}
