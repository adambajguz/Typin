﻿namespace Typin.Models.Schemas
{
    using System;
    using Typin.Models.Binding;
    using Typin.Schemas;

    /// <summary>
    /// Argument schema.
    /// </summary>
    public interface IArgumentSchema : ISchema
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