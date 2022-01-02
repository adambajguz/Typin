namespace Typin.Pipeline
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin;
    using Typin.Commands.Collections;
    using Typin.Commands.Schemas;
    using Typin.Features.Input;
    using Typin.Models.Schemas;

    /// <summary>
    /// Parses input.
    /// </summary>
    public sealed class ParseInput : IMiddleware
    {
        private readonly ICommandSchemaCollection _commandSchemas;

        /// <summary>
        /// Initializes a new instance of <see cref="BindInput"/>.
        /// </summary>
        public ParseInput(ICommandSchemaCollection commandSchemas)
        {
            _commandSchemas = commandSchemas;
        }

        /// <inheritdoc/>
        public async ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            bool trimExecutable = args.Input.Options.HasFlag(InputOptions.TrimExecutable);

            ParsedInput input = Parse(args.Input.Arguments.Skip(trimExecutable ? 1 : 0));

            args.Input.Parsed = input;

            await next();
        }

        /// <summary>
        /// Resolves <see cref="ParsedInput"/>.
        /// </summary>
        public ParsedInput Parse(IEnumerable<string> commandLineArguments)
        {
            int index = 0;

            IReadOnlyList<string> tmp = commandLineArguments.ToList();

            IReadOnlyList<DirectiveInput> directives = ParseDirectives(
                tmp,
                ref index
            );

            string commandName = ParseCommandName(
                tmp,
                ref index
            );

            IReadOnlyList<ParameterInput> parameters = ParseParameters(
                tmp,
                ref index
            );

            IReadOnlyList<OptionInput> options = ParseOptions(
                tmp,
                ref index
            );

            return new ParsedInput(directives, commandName, parameters, options);
        }

        private static IReadOnlyList<DirectiveInput> ParseDirectives(IReadOnlyList<string> commandLineArguments,
                                                                     ref int index)
        {
            List<DirectiveInput> result = new();

            for (; index < commandLineArguments.Count; index++)
            {
                string argument = commandLineArguments[index];

                if (!argument.StartsWith('[') || !argument.EndsWith(']'))
                {
                    break;
                }

                string name = argument[1..^1];

                result.Add(new DirectiveInput(name));
            }

            return result;
        }

        private string ParseCommandName(IReadOnlyList<string> commandLineArguments,
                                        ref int index)
        {
            List<string> buffer = new();

            string? commandName = null;
            int lastIndex = index;

            // We need to look ahead to see if we can match as many consecutive arguments to a command name as possible
            for (int i = index; i < commandLineArguments.Count; ++i)
            {
                string argument = commandLineArguments[i];
                buffer.Add(argument);

                string potentialCommandName = string.Join(' ', buffer);

                if (_commandSchemas.Get(potentialCommandName) is ICommandSchema potentialCommandSchema)
                {
                    commandName = potentialCommandName;
                    lastIndex = i;
                }
            }

            // Update the index only if command name was found in the arguments
            if (!string.IsNullOrWhiteSpace(commandName))
            {
                index = lastIndex + 1;
            }

            return commandName?.Trim() ?? string.Empty;
        }

        private static IReadOnlyList<ParameterInput> ParseParameters(IReadOnlyList<string> commandLineArguments,
                                                                     ref int index)
        {
            List<ParameterInput> result = new();

            for (; index < commandLineArguments.Count; index++)
            {
                string argument = commandLineArguments[index];

                if (IOptionSchema.IsName(argument) || IOptionSchema.IsShortName(argument))
                {
                    break;
                }

                result.Add(new ParameterInput(argument));
            }

            return result;
        }

        private static IReadOnlyList<OptionInput> ParseOptions(IReadOnlyList<string> commandLineArguments,
                                                               ref int index)
        {
            List<OptionInput> result = new();

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
                        result.Add(new OptionInput(currentOptionAlias, currentOptionValues));
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
                            result.Add(new OptionInput(currentOptionAlias, currentOptionValues));
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
                result.Add(new OptionInput(currentOptionAlias, currentOptionValues));
            }

            return result;
        }
    }
}
