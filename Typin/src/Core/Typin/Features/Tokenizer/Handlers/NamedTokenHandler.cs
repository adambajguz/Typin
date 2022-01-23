namespace Typin.Features.Tokenizer.Handlers
{
    using System;
    using System.Collections.Generic;
    using Typin.Features.Input;
    using Typin.Features.Input.Tokens;
    using Typin.Models.Schemas;

    /// <summary>
    /// <see cref="NamedToken"/> handler.
    /// </summary>
    public sealed class NamedTokenHandler : ITokenHandler<NamedToken>
    {
        /// <inheritdoc/>
        public bool CanHandle(TokenHandlerContext context)
        {
            return true;
        }

        /// <inheritdoc/>
        public bool Handle(TokenHandlerContext context, TokenGroup<NamedToken> tokenGroup)
        {
            throw new NotImplementedException();
        }

        private static void ParseNamedValues(ITokenCollection input,
                                             IReadOnlyList<string> commandLineArguments,
                                             bool explicitlyOpenedDirective,
                                             ref int index)
        {
            TokenGroup<NamedToken> tokenGroup = input.Get<NamedToken>() ?? new();

            string? currentOptionAlias = null;
            List<string> currentOptionValues = new();
            bool finish = false;

            for (; index < commandLineArguments.Count; index++)
            {
                string argument = commandLineArguments[index];

                if (explicitlyOpenedDirective && argument.EndsWith(']'))
                {
                    argument = argument[..^1];
                    finish = true;
                }

                // Name
                if (IOptionSchema.IsName(argument))
                {
                    // Flush previous
                    if (!string.IsNullOrWhiteSpace(currentOptionAlias))
                    {
                        tokenGroup.Tokens.Add(new NamedToken(currentOptionAlias, currentOptionValues));
                    }

                    currentOptionAlias = argument[2..];
                    currentOptionValues = new List<string>();
                }
                // Short name
                else if (IOptionSchema.IsShortName(argument))
                {
                    foreach (var alias in argument[1..])
                    {
                        // Flush previous
                        if (!string.IsNullOrWhiteSpace(currentOptionAlias))
                        {
                            tokenGroup.Tokens.Add(new NamedToken(currentOptionAlias, currentOptionValues));
                        }

                        currentOptionAlias = alias.ToString();
                        currentOptionValues = new List<string>();
                    }
                }
                // Value
                else if (!string.IsNullOrWhiteSpace(currentOptionAlias))
                {
                    currentOptionValues.Add(argument);
                }

                if (finish)
                {
                    break;
                }
            }

            // Flush last option
            if (!string.IsNullOrWhiteSpace(currentOptionAlias))
            {
                tokenGroup.Tokens.Add(new NamedToken(currentOptionAlias, currentOptionValues));
            }

            if (tokenGroup.Tokens.Count > 0 && input.Get(typeof(NamedToken)) is null)
            {
                input.Set(tokenGroup);
            }
        }
    }
}
