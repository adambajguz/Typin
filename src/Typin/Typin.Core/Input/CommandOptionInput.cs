namespace Typin.Input
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Typin.Internal.Extensions;
    using Typin.Schemas;

    /// <summary>
    /// Stores command option input.
    /// </summary>
    public class CommandOptionInput
    {
        /// <summary>
        /// Option alias.
        /// </summary>
        public string Alias { get; }

        /// <summary>
        /// Option values.
        /// </summary>
        public IReadOnlyList<string> Values { get; }

        /// <summary>
        /// Whether option is help option (--help|-h).
        /// </summary>
        public bool IsHelpOption => OptionSchema.HelpOption.MatchesNameOrShortName(Alias);

        /// <summary>
        /// Whether option is version option (--version).
        /// </summary>
        public bool IsVersionOption => OptionSchema.VersionOption.MatchesNameOrShortName(Alias);

        /// <summary>
        /// Initializes an instance of <see cref="CommandOptionInput"/>.
        /// </summary>
        public CommandOptionInput(string alias, IReadOnlyList<string> values)
        {
            Alias = alias;
            Values = values;
        }

        /// <summary>
        /// Gets raw alias.
        /// </summary>
        public string GetRawAlias()
        {
            return Alias switch
            {
                { Length: 0 } => Alias,
                { Length: 1 } => $"-{Alias}",
                _ => $"--{Alias}"
            };
        }

        /// <summary>
        /// Gets raw values.
        /// </summary>
        public string GetRawValues()
        {
            return Values.Select(v => v.Quote()).JoinToString(' ');
        }

        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            return $"{GetRawAlias()} {GetRawValues()}";
        }

        /// <summary>
        /// Checks whether text is a valid option.
        /// </summary>
        public static bool IsOption(string argument)
        {
            return argument.StartsWith("--", StringComparison.Ordinal) && argument.Length > 3 && char.IsLetter(argument[2]);
        }

        /// <summary>
        /// Checks whether text is a valid option alias.
        /// </summary>
        public static bool IsOptionAlias(string argument)
        {
            return argument.StartsWith('-') && argument.Length >= 2 && char.IsLetter(argument[1]);
        }
    }
}