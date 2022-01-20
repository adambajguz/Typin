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
        private int _idSource;

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
        public IDirectiveCollection Tokenize(IEnumerable<string> commandLineArguments)
        {
            int index = 0;

            DirectiveCollection directives = new();
            List<string> tmp = commandLineArguments.ToList();

            ParseDirectives(directives, tmp, ref index);

            return directives;
        }

        private void ParseDirectives(IDirectiveCollection directives,
                                     IReadOnlyList<string> args,
                                     ref int index)
        {
            for (; index < args.Count; index++)
            {
                string argument = args[index].Trim();

                bool isExplicitlyOpened = argument.StartsWith('[');
                bool isTerminated = argument.EndsWith(']');

                bool isName = (isExplicitlyOpened && isTerminated) || argument.EndsWith(':');
                string directiveName = isName ? argument : string.Empty;

                int id = Interlocked.Increment(ref _idSource);
                DirectiveToken directiveToken = new(id, directiveName);
                directives.Add(directiveToken);

                if (!isTerminated)
                {
                    ITokenCollection childTokens = directiveToken.Children;

                    ParseValues(childTokens, args, isExplicitlyOpened, ref index);
                    ParseNamedValues(childTokens, args, isExplicitlyOpened, ref index);
                }
            }
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
