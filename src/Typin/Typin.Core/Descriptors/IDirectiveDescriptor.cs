namespace Typin.Descriptors
{
    using System;

    /// <summary>
    /// Directive descriptor.
    /// </summary>
    public interface IDirectiveDescriptor : IDescriptor
    {
        /// <summary>
        /// Directive name.
        /// All directives in a command must have different names.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Directive description, which is used in help text.
        /// </summary>
        string? Description { get; }

        /// <summary>
        /// List of CLI mode types, in which the directive can be executed.
        /// If null (default) or empty, directive can be executed in every registered mode in the app.
        /// </summary>
        Type[]? SupportedModes { get; }

        /// <summary>
        /// List of CLI mode types, in which the directive cannot be executed.
        /// If null (default) or empty, directive can be executed in every registered mode in the app.
        /// </summary>
        Type[]? ExcludedModes { get; }
    }
}