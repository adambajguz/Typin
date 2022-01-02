namespace Typin.Models.Attributes
{
    using System;

    /// <summary>
    /// Annotates a property that defines a command option.
    /// </summary>
    public sealed class OptionAttribute : ArgumentAttribute
    {
        /// <summary>
        /// Option name (must be longer than a single character). Starting dashes are trimed automatically.
        /// All options in a command must have different names (comparison is case-insensitive).
        /// If this isn't specified, kebab-cased property name is used instead.
        /// </summary>
        public string? Name { get; init; }

        /// <summary>
        /// Option short name (single character).
        /// All options in a command must have different short names (comparison is case-insensitive).
        /// </summary>
        public char? ShortName { get; init; }

        /// <summary>
        /// Whether an option is required.
        /// </summary>
        public bool IsRequired { get; init; }

        /// <summary>
        /// Option description, which is used in help text.
        /// </summary>
        public string? Description { get; init; }

        /// <summary>
        /// Binding converter.
        /// </summary>
        public Type? Converter { get; init; }

        /// <summary>
        /// Initializes an instance of <see cref="OptionAttribute"/>.
        /// </summary>
        private OptionAttribute(string? name, char? shortName)
        {
            Name = name;
            ShortName = shortName;
        }

        /// <summary>
        /// Initializes an instance of <see cref="OptionAttribute"/>.
        /// </summary>
        public OptionAttribute(string name, char shortName)
            : this(name, (char?)shortName)
        {

        }

        /// <summary>
        /// Initializes an instance of <see cref="OptionAttribute"/>.
        /// </summary>
        public OptionAttribute(string name)
            : this(name, null)
        {

        }

        /// <summary>
        /// Initializes an instance of <see cref="OptionAttribute"/>.
        /// </summary>
        public OptionAttribute()
            : this(null, null)
        {

        }

        /// <summary>
        /// Initializes an instance of <see cref="OptionAttribute"/>.
        /// </summary>
        public OptionAttribute(char shortName)
            : this(null, (char?)shortName)
        {

        }
    }
}