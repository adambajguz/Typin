namespace Typin.Models.Attributes
{
    using System;

    /// <summary>
    /// Annotates a property that defines a command parameter.
    /// </summary>
    public sealed class ParameterAttribute : ArgumentAttribute
    {
        /// <inheritdoc/>
        public int? Order { get; init; }

        /// <summary>
        /// Parameter name, which is only used in help text.
        /// If this isn't specified, kebab-cased property name is used instead.
        /// </summary>
        public string? Name { get; init; }

        /// <summary>
        /// Parameter description, which is used in help text.
        /// </summary>
        public string? Description { get; init; }

        /// <inheritdoc/>
        public Type? Converter { get; init; }

        /// <summary>
        /// Initializes an instance of <see cref="ParameterAttribute"/>.
        /// </summary>
        public ParameterAttribute()
        {

        }

        /// <summary>
        /// Initializes an instance of <see cref="ParameterAttribute"/>.
        /// </summary>
        public ParameterAttribute(int order)
        {
            Order = order;
        }
    }
}
