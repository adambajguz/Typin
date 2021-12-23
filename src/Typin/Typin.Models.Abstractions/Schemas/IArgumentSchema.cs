namespace Typin.Models.Schemas
{
    using System;
    using Typin.Models.Binding;

    /// <summary>
    /// Argument schema.
    /// </summary>
    public interface IArgumentSchema
    {
        /// <summary>
        /// Bindable argument.
        /// </summary>
        IBindableArgument Bindable { get; }

        /// <summary>
        /// Argument converter type.
        /// </summary>
        Type? ConverterType { get; init; }

        /// <summary>
        /// Argument name.
        /// </summary>
        string? Name { get; }

        /// <summary>
        /// Optional argument description.
        /// </summary>
        string? Description { get; }
    }
}