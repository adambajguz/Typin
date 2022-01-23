namespace Typin.Features.Tokenizer.Handlers
{
    using System;
    using System.Collections.Generic;
    using Typin.Features.Input;
    using Typin.Features.Input.Tokens;
    using Typin.Models.Schemas;

    /// <summary>
    /// <see cref="ValueToken"/> handler.
    /// </summary>
    public sealed class ValueTokenHandler : ITokenHandler<ValueToken>
    {
        /// <inheritdoc/>
        public bool CanHandle(TokenHandlerContext context)
        {
            return true;
        }

        /// <inheritdoc/>
        public bool Handle(TokenHandlerContext context, TokenGroup<ValueToken> tokenGroup)
        {
            throw new NotImplementedException();
        }

        private static void ParseValues(ITokenCollection input,
                                        IReadOnlyList<string> commandLineArguments,
                                        bool explicitlyOpenedDirective,
                                        ref int index)
        {
            TokenGroup<ValueToken> tokenGroup = input.Get<ValueToken>() ?? new();

            for (; index < commandLineArguments.Count; index++)
            {
                string argument = commandLineArguments[index];

                if (IOptionSchema.IsName(argument) ||
                    IOptionSchema.IsShortName(argument))
                {
                    break;
                }

                if (explicitlyOpenedDirective && argument.EndsWith(']'))
                {
                    tokenGroup.Tokens.Add(new ValueToken(argument[..^1]));

                    break;
                }

                tokenGroup.Tokens.Add(new ValueToken(argument));
            }

            if (tokenGroup.Tokens.Count > 0 && input.Get(typeof(ValueToken)) is null)
            {
                input.Set(tokenGroup);
            }
        }
    }
}
