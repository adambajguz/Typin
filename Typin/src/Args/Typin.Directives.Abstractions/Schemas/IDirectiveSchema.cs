namespace Typin.Directives.Schemas
{
    using System;
    using Typin.Models.Schemas;
    using Typin.Schemas;

    /// <summary>
    /// Directive schema.
    /// </summary>
    public interface IDirectiveSchema : ISchema
    {
        /// <summary>
        /// Directive name.
        /// All directives in an application must have different names.
        /// </summary>
        string Name { get; }

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