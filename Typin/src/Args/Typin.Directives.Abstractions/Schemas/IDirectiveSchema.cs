namespace Typin.Directives.Schemas
{
    using System;
    using Typin.Models.Schemas;
    using Typin.Schemas;

    /// <summary>
    /// Directive schema.
    /// </summary>
    public interface IDirectiveSchema : IAliasableSchema
    {
        /// <summary>
        /// Whether directive is a default directive.
        /// </summary>
        bool IsDefault { get; }

        /// <summary>
        /// Directive description, which is used in help text.
        /// </summary>
        string? Description { get; }

        /// <summary>
        /// Directive handler type.
        /// </summary>
        Type Handler { get; }

        /// <summary>
        /// Directive model schema.
        /// </summary>
        IModelSchema Model { get; }
    }
}