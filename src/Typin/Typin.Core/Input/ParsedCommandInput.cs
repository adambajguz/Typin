namespace Typin.Input
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using Typin.Internal.Extensions;

    /// <summary>
    /// Provides a command parser and command class represention.
    /// <remarks>
    /// Command schema is `{directives} {command name} {parameters} {options}`.
    /// </remarks>
    /// </summary>
    public sealed class ParsedCommandInput
    {
        /// <summary>
        /// Raw command line input arguments.
        /// </summary>
        public IReadOnlyList<string> Arguments { get; }

        /// <summary>
        /// Command direcitves list without special [interactive] directive.
        /// </summary>
        public IReadOnlyList<DirectiveInput> Directives { get; }

        /// <summary>
        /// Command name. Null or empty/whitespace if default or invalid command.
        /// </summary>
        public string? CommandName { get; }

        /// <summary>
        /// Command parameters list.
        /// </summary>
        public IReadOnlyList<CommandParameterInput> Parameters { get; }

        /// <summary>
        /// Command options list.
        /// </summary>
        public IReadOnlyList<CommandOptionInput> Options { get; }

        /// <summary>
        /// Whether command has help option specified (--help|-h).
        /// </summary>
        public bool IsHelpOptionSpecified => Options.Any(o => o.IsHelpOption);

        /// <summary>
        /// Whether command has version option specified (--version).
        /// </summary>
        public bool IsVersionOptionSpecified => Options.Any(o => o.IsVersionOption);

        /// <summary>
        /// Whether command input is default command or empty (no command name, no options, no parameters, and no directives.
        /// </summary>
        public bool IsDefaultCommandOrEmpty { get; }

        /// <summary>
        /// Initializes an instance of <see cref="ParsedCommandInput"/>.
        /// </summary>
        public ParsedCommandInput(IReadOnlyList<string> commandLineArguments,
                            IReadOnlyList<DirectiveInput> directives,
                            string? commandName,
                            IReadOnlyList<CommandParameterInput> parameters,
                            IReadOnlyList<CommandOptionInput> options)
        {
            Arguments = commandLineArguments;
            Directives = directives;
            CommandName = commandName;
            Parameters = parameters;
            Options = options;

            IsDefaultCommandOrEmpty = Options.Count == 0 &&
                                      Parameters.Count == 0 &&
                                      Directives.Count == 0 &&
                                      string.IsNullOrWhiteSpace(CommandName);
        }

        /// <summary>
        /// Whether command has a directive.
        /// </summary>
        public bool HasDirective(string directive)
        {
            string v = directive.Trim('[', ']')
                                .ToLower();

            return Directives.Where(x => x.Name == v)
                             .Any();
        }

        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            StringBuilder buffer = new();

            foreach (DirectiveInput directive in Directives)
            {
                buffer.AppendIfNotEmpty(' ')
                      .Append(directive);
            }

            if (!string.IsNullOrWhiteSpace(CommandName))
            {
                buffer.AppendIfNotEmpty(' ')
                      .Append(CommandName);
            }

            foreach (CommandParameterInput parameter in Parameters)
            {
                buffer.AppendIfNotEmpty(' ')
                      .Append(parameter);
            }

            foreach (CommandOptionInput option in Options)
            {
                buffer.AppendIfNotEmpty(' ')
                      .Append(option);
            }

            return buffer.ToString();
        }
    }
}