namespace Typin.Input
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using Typin.Schemas;
    using Typin.Utilities.Extensions;

    /// <summary>
    /// Provides a command parser and command class represention.
    /// <remarks>
    /// Command schema is `{directives} {command name} {parameters} {options}`.
    /// </remarks>
    /// </summary>
    public sealed class ParsedCommandInput
    {
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
        public ParsedCommandInput(IReadOnlyList<DirectiveInput> directives,
                                  string? commandName,
                                  IReadOnlyList<CommandParameterInput> parameters,
                                  IReadOnlyList<CommandOptionInput> options)
        {
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
            string v = directive.Trim('[', ']');

            return Directives.Where(x => x.Name == v)
                             .Any();
        }

        #region With
        /// <summary>
        /// Creates a new instance of <see cref="ParsedCommandInput"/> with directives specified in parameter <paramref name="directives"/>.
        /// </summary>
        /// <param name="directives"></param>
        /// <returns></returns>
        public ParsedCommandInput WithDirectives(IEnumerable<DirectiveInput> directives)
        {
            return new ParsedCommandInput(directives.ToList(),
                                          CommandName,
                                          Parameters,
                                          Options);
        }

        /// <summary>
        /// Creates a new instance of <see cref="ParsedCommandInput"/> without a directive specified in parameter <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ParsedCommandInput WithoutDirective(string name)
        {
            return WithDirectives(Directives.Where(x => !string.Equals(x.Name, name.Trim('[', ']'), StringComparison.Ordinal)));
        }

        /// <summary>
        /// Creates a new instance of <see cref="ParsedCommandInput"/> with command name specified in parameter <paramref name="commandName"/>.
        /// </summary>
        /// <param name="commandName"></param>
        /// <returns></returns>
        public ParsedCommandInput WithCommandName(string? commandName)
        {
            return new ParsedCommandInput(Directives,
                                          commandName?.Trim(),
                                          Parameters,
                                          Options);
        }

        /// <summary>
        /// Creates a new instance of <see cref="ParsedCommandInput"/> with parameters specified in parameter <paramref name="parameters"/>.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public ParsedCommandInput WithParameters(IEnumerable<CommandParameterInput> parameters)
        {
            return new ParsedCommandInput(Directives,
                                          CommandName,
                                          parameters.ToList(),
                                          Options);
        }

        /// <summary>
        /// Creates a new instance of <see cref="ParsedCommandInput"/> with options specified in parameter <paramref name="options"/>.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public ParsedCommandInput WithOptions(IEnumerable<CommandOptionInput> options)
        {
            return new ParsedCommandInput(Directives,
                                          CommandName,
                                          Parameters,
                                          options.ToList());
        }

        /// <summary>
        /// Creates a new instance of <see cref="ParsedCommandInput"/> with help option.
        /// </summary>
        /// <returns></returns>
        public ParsedCommandInput WithHelp()
        {
            CommandOptionInput helpOption = new(OptionSchema.HelpOption.GetCallName());

            return new ParsedCommandInput(Directives,
                                          CommandName,
                                          Parameters,
                                          Options.Concat(new[] { helpOption }).ToList());
        }

        /// <summary>
        /// Creates a new instance of <see cref="ParsedCommandInput"/> with version option.
        /// </summary>
        /// <returns></returns>
        public ParsedCommandInput WithVersion()
        {
            CommandOptionInput versionOption = new(OptionSchema.VersionOption.GetCallName());

            return new ParsedCommandInput(Directives,
                                          CommandName,
                                          Parameters,
                                          Options.Concat(new[] { versionOption }).ToList());
        }
        #endregion

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