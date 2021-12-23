namespace Typin.Models.Schemas
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using Typin.Models.Collections;

    /// <summary>
    /// Stores command option schema.
    /// </summary>
    public class OptionSchema : ArgumentSchema, IOptionSchema
    {
        /// <inheritdoc/>
        public char? ShortName { get; }

        /// <inheritdoc/>
        public bool IsRequired { get; }

        /// <inheritdoc/>
        public string? FallbackVariableName { get; }

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
            : base(property, name, description, converter, metadata)
        {
            ShortName = shortName;
            FallbackVariableName = fallbackVariableName;
            IsRequired = isRequired;

            if (shortName is null && name is null)
            {
                throw new ArgumentException($"Both {nameof(name)} and {nameof(shortName)} cannot be null.");
            }
        }

        /// <summary>
        /// Initializes an instance of <see cref="OptionSchema"/> that represents a dynamic.
        /// </summary>
        public OptionSchema(Type propertyType,
                            string propertyName,
                            string? name,
                            char? shortName,
                            string? fallbackVariableName,
                            bool isRequired,
                            string? description,
                            Type? converter,
                            IMetadataCollection metadata)
            : base(propertyType, propertyName, name, description, converter, metadata)
        {
            ShortName = shortName;
            FallbackVariableName = fallbackVariableName;
            IsRequired = isRequired;

            if (shortName is null && name is null)
            {
                throw new ArgumentException($"Both {nameof(name)} and {nameof(shortName)} cannot be null.");
            }
        }

        /// <inheritdoc/>
        public bool MatchesName(string name)
        {
            return !string.IsNullOrWhiteSpace(Name) && string.Equals(Name, name, StringComparison.Ordinal);
        }

        /// <inheritdoc/>
        public bool MatchesShortName(char shortName)
        {
            return ShortName is not null && ShortName == shortName;
        }

        /// <inheritdoc/>
        public string GetCallName()
        {
            return Name is null ? $"-{ShortName}" : $"--{Name}";
        }

        /// <inheritdoc/>
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