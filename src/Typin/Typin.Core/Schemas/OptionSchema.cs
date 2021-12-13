namespace Typin.Schemas
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using Typin.Metadata;

    /// <summary>
    /// Stores command option schema.
    /// </summary>
    public class OptionSchema : ArgumentSchema
    {
        private static readonly IMetadataCollection _emptyMetadata = new MetadataCollection();

        /// <summary>
        /// Gets a help option schema instance.
        /// </summary>
        public static OptionSchema HelpOption { get; } = new(typeof(bool),
                                                             "  __ShowHelp",
                                                             false,
                                                             "help",
                                                             'h',
                                                             null,
                                                             false,
                                                             "Shows help text.",
                                                             null,
                                                             _emptyMetadata);

        /// <summary>
        /// Gets a version option schema instance.
        /// </summary>
        public static OptionSchema VersionOption { get; } = new(typeof(bool),
                                                                "  __ShowVersion",
                                                                false,
                                                                "version",
                                                                null,
                                                                null,
                                                                false,
                                                                "Shows version information.",
                                                                null,
                                                                _emptyMetadata);

        /// <summary>
        /// Option name.
        /// </summary>
        public string? Name { get; }

        /// <summary>
        /// Option short name.
        /// </summary>
        public char? ShortName { get; }

        /// <summary>
        /// Name of variable used as a fallback value.
        /// </summary>
        public string? FallbackVariableName { get; }

        /// <summary>
        /// Whether option is required.
        /// </summary>
        public bool IsRequired { get; }

        /// <summary>
        /// Initializes an instance of <see cref="OptionSchema"/> that represents a property-based option.
        /// </summary>
        public OptionSchema(PropertyInfo property,
                            string? name,
                            char? shortName,
                            string? fallbackVariableName,
                            bool isRequired,
                            string? description,
                            Type? converter,
                            IMetadataCollection metadata)
            : base(property, description, converter, metadata)
        {
            Name = name;
            ShortName = shortName;
            FallbackVariableName = fallbackVariableName;
            IsRequired = isRequired;

            if (shortName is null && name is null)
            {
                throw new ArgumentException($"Both {nameof(name)} and {nameof(shortName)} cannot be null.");
            }
        }

        /// <summary>
        /// Initializes an instance of <see cref="OptionSchema"/> that represents a dynamic or built-in option.
        /// </summary>
        public OptionSchema(Type propertyType,
                            string propertyName,
                            bool isDynamic,
                            string? name,
                            char? shortName,
                            string? fallbackVariableName,
                            bool isRequired,
                            string? description,
                            Type? converter,
                            IMetadataCollection metadata)
            : base(propertyType, propertyName, isDynamic, description, converter, metadata)
        {
            Name = name;
            ShortName = shortName;
            FallbackVariableName = fallbackVariableName;
            IsRequired = isRequired;

            if (shortName is null && name is null)
            {
                throw new ArgumentException($"Both {nameof(name)} and {nameof(shortName)} cannot be null.");
            }
        }

        /// <summary>
        /// Whether command's name matches the passed name.
        /// </summary>
        public bool MatchesName(string name)
        {
            return !string.IsNullOrWhiteSpace(Name) && string.Equals(Name, name, StringComparison.Ordinal);
        }

        /// <summary>
        /// Whether command's short name matches the passed short name.
        /// </summary>
        public bool MatchesShortName(char shortName)
        {
            return ShortName is not null && ShortName == shortName;
        }

        /// <summary>
        /// Gets a call name.
        /// </summary>
        /// <returns></returns>
        public string GetCallName()
        {
            return Name is null ? $"-{ShortName}" : $"--{Name}";
        }

        /// <summary>
        /// Whether command's name or short name matches the passed name.
        /// </summary>
        public bool MatchesNameOrShortName(string alias)
        {
            return MatchesName(alias) || alias.Length == 1 && MatchesShortName(alias.Single());
        }

        private string GetUserFacingDisplayString()
        {
            StringBuilder buffer = new();

            if (!string.IsNullOrWhiteSpace(Name))
            {
                buffer.Append("--")
                      .Append(Name);
            }

            if (!string.IsNullOrWhiteSpace(Name) && ShortName is not null)
            {
                buffer.Append('|');
            }

            if (ShortName is not null)
            {
                buffer.Append('-')
                      .Append(ShortName);
            }

            return buffer.ToString();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{Bindable.Name} ('{GetUserFacingDisplayString()}')";
        }
    }
}