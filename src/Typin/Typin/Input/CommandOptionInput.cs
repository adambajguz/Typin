﻿namespace Typin.Input
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
        public bool IsHelpOption => CommandOptionSchema.HelpOption.MatchesNameOrShortName(Alias);

        /// <summary>
        /// Whether option is version option (--version).
        /// </summary>
        public bool IsVersionOption => CommandOptionSchema.VersionOption.MatchesNameOrShortName(Alias);

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


        internal static bool IsOption(string argument)
        {
            return argument.StartsWith("--", StringComparison.Ordinal) && argument.Length >= 3;
        }

        internal static bool IsOptionAlias(string argument)
        {
            return argument.StartsWith('-') && argument.Length >= 2 && !char.IsDigit(argument[1]);
        }
    }
}