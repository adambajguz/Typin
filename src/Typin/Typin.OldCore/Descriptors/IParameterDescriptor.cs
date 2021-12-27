namespace Typin.Descriptors
{
    using System;

    /// <summary>
    /// Command parameter descriptor.
    /// </summary>
    public interface IParameterDescriptor : IDescriptor
    {
        /// <summary>
        /// Order of this parameter compared to other parameters.
        /// All parameters in a command must have different order.
        /// Parameter whose type is a non-scalar (e.g. array), must be the last in order and only one such parameter is allowed.
        /// </summary>
        int Order { get; }

        /// <summary>
        /// Parameter name, which is only used in help text.
        /// If this isn't specified, kebab-cased property name is used instead.
        /// </summary>
        string? Name { get; }

        /// <summary>
        /// Parameter description, which is used in help text.
        /// </summary>
        string? Description { get; }

        /// <summary>
        /// Binding converter.
        /// </summary>
        Type? Converter { get; }
    }
}