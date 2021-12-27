namespace Typin.Features.Input
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Stores an option input.
    /// </summary>
    public sealed record OptionInput
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
        /// Initializes an instance of <see cref="OptionInput"/>.
        /// </summary>
        public OptionInput(string alias)
        {
            Alias = alias;
            Values = new List<string>();
        }

        /// <summary>
        /// Initializes an instance of <see cref="OptionInput"/>.
        /// </summary>
        public OptionInput(string alias, IReadOnlyList<string> values)
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
            return CommandLine.EncodeArguments(Values);
        }

        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            return string.Concat(GetRawAlias(), " ", GetRawValues());
        }
    }
}