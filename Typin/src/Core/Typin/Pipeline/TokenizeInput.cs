namespace Typin.Pipeline
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin;
    using Typin.Commands.Collections;
    using Typin.Features.Input;
    using Typin.Features.Input.Tokens;
    using Typin.Models.Schemas;

    /// <summary>
    /// Tokenizes the input.
    /// </summary>
    public sealed class TokenizeInput : IMiddleware
    {
        private readonly ICommandSchemaCollection _commandSchemas;

        /// <summary>
        /// Initializes a new instance of <see cref="BindInput"/>.
        /// </summary>
        public TokenizeInput(ICommandSchemaCollection commandSchemas)
        {
            _commandSchemas = commandSchemas;
        }

        /// <inheritdoc/>
        public async ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            bool trimExecutable = args.Input.Options.HasFlag(InputOptions.TrimExecutable);

            args.Input.Tokens = Tokenize(args.Input.Original.Skip(trimExecutable ? 1 : 0));

            await next();
        }

        /// <summary>
        /// Resolves <see cref="ITokenCollection"/>.
        /// </summary>
        public static IDirectiveCollection Tokenize(IEnumerable<string> commandLineArguments)
        {
            int index = 0;

            DirectiveCollection directivesGroup = new();
            List<string> tmp = commandLineArguments.ToList();

            ParseDirectives(directivesGroup, tmp, ref index);

            return directivesGroup;
        }

        private static void ParseDirectives(IDirectiveCollection directivesGroup,
                                            IReadOnlyList<string> args,
                                            ref int index)
        {
            for (; index < args.Count; index++)
            {
                string argument = args[index].Trim();

                bool isTerminated = argument.EndsWith(']');
                bool isName = (argument.StartsWith('[') && isTerminated) ||
                    argument.EndsWith(':');

                string directiveName = string.Empty;

                if (isName)
                {
                    directiveName = argument;
                    index++;

                    argument = args[index].Trim();
                }

                DirectiveToken directiveToken = new(directiveName);
                directivesGroup.Add(directiveToken);

                if (!isTerminated)
                {
                    ITokenCollection childTokens = new TokenCollection();

                    ParseValues(childTokens, args, ref index);
                    ParseNamedValues(childTokens, args, ref index);

                    if (childTokens.Count > 0)
                    {
                        directiveToken.Children = childTokens;
                    }
                }
            }
        }

        private static void ParseValues(ITokenCollection input,
                                        IReadOnlyList<string> commandLineArguments,
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

                tokenGroup.Tokens.Add(new ValueToken(argument));
            }

            if (tokenGroup.Tokens.Count > 0 && input.Get(typeof(ValueToken)) is null)
            {
                input.Set(tokenGroup);
            }
        }

        private static void ParseNamedValues(ITokenCollection input,
                                             IReadOnlyList<string> commandLineArguments,
                                             ref int index)
        {
            TokenGroup<NamedToken> tokenGroup = input.Get<NamedToken>() ?? new();

            string? currentOptionAlias = null;
            List<string> currentOptionValues = new();

            for (; index < commandLineArguments.Count; index++)
            {
                string argument = commandLineArguments[index];

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
