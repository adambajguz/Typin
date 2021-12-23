namespace Typin.Descriptors
{
    using System;

    /// <summary>
    /// Command option descriptor.
    /// </summary>
    public interface IOptionDescriptor : IDescriptor
    {
        /// <summary>
        /// Option name (must be longer than a single character). Starting dashes are trimed automatically.
        /// All options in a command must have different names (comparison is case-sensitive).
        /// </summary>
        string? Name { get; }

        /// <summary>
        /// Option short name (single character).
        /// All options in a command must have different short names (comparison is case-sensitive).
        /// </summary>
        char? ShortName { get; }

        /// <summary>
        /// Whether an option is required.
        /// </summary>
        bool IsRequired { get; }

        /// <summary>
        /// Option description, which is used in help text.
        /// </summary>
        string? Description { get; } //TODO: rename to summary

        /// <summary>
        /// Fallback variable that will be used as fallback if no option value is specified.
        /// </summary>
        string? FallbackVariableName { get; }

        /// <summary>
        /// Binding converter.
        /// </summary>
        Type? Converter { get; }
    }
}