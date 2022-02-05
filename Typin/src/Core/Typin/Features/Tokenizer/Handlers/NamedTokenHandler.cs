namespace Typin.Features.Tokenizer.Handlers
{
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
            if (context.Position >= context.Arguments.Count)
            {
                return false;
            }

            string argument = context.Arguments[context.Position];

            return IOptionSchema.IsName(argument) ||
                IOptionSchema.IsShortNameGroup(argument);
        }

        /// <inheritdoc/>
        public bool Handle(TokenHandlerContext context, TokenGroup<NamedToken> tokenGroup)
        {
            if (context.Position >= context.Arguments.Count)
            {
                return false;
            }

            string optionName = context.Arguments[context.Position++];

            // Name
            if (IOptionSchema.IsName(optionName))
            {
                bool hasDirectiveTermination = context.Directive.IsExplicit && optionName.EndsWith(']');

                optionName = hasDirectiveTermination
                    ? optionName[..^1]
                    : optionName;

                List<string> values = hasDirectiveTermination
                    ? new()
                    : TokenizeOptionValues(context);

                tokenGroup.Tokens.Add(new NamedToken(optionName, values));

                return true;
            }

            // Short name
            if (IOptionSchema.IsShortNameGroup(optionName))
            {
                bool hasDirectiveTermination = context.Directive.IsExplicit && optionName.EndsWith(']');

                string shortNamesCollection = hasDirectiveTermination
                    ? optionName[1..^1]
                    : optionName[1..];

                foreach (char shortName in shortNamesCollection)
                {
                    tokenGroup.Tokens.Add(new NamedToken(shortName.ToString(), new List<string>()));
                }

                List<string> values = hasDirectiveTermination
                    ? new()
                    : TokenizeOptionValues(context);

                return true;
            }

            return false;
        }

        private static List<string> TokenizeOptionValues(TokenHandlerContext context)
        {
            List<string> values = new();

            int index;
            for (index = context.Position; index < context.Arguments.Count; index++)
            {
                string argument = context.Arguments[index++];

                if (IOptionSchema.IsName(argument) ||
                    IOptionSchema.IsShortNameGroup(argument))
                {
                    break;
                }

                bool hasDirectiveTermination = context.Directive.IsExplicit && argument.EndsWith(']');

                argument = hasDirectiveTermination
                    ? argument[..^1]
                    : argument;

                values.Add(argument);

                if (hasDirectiveTermination)
                {
                    break;
                }
            }

            context.Position = index;

            return values;
        }
    }
}
