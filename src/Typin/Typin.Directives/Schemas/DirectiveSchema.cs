namespace Typin.Directives.Schemas
{
    using System;
    using Typin.Models.Schemas;
    using Typin.Schemas.Collections;

    /// <summary>
    /// Stores directive schema.
    /// </summary>
    public class DirectiveSchema : IDirectiveSchema
    {
        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public string? Description { get; }

        /// <inheritdoc/>
        public IModelSchema Model { get; }

        /// <inheritdoc/>
        public Type Handler { get; }

        /// <inheritdoc/>
        public IExtensionsCollection Extensions { get; }

        /// <summary>
        /// Initializes an instance of <see cref="DirectiveSchema"/>.
        /// </summary>
        public DirectiveSchema(string name,
                               string? description,
                               IModelSchema model,
                               Type handler,
                               IExtensionsCollection extensions)
        {
            Name = name;
            Description = description;
            Model = model;
            Handler = handler;
            Extensions = extensions;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return base.ToString() +
                " | " +
                $"{nameof(Model)}.{nameof(IModelSchema.Type)} = {Model.Type}, " +
                $"{nameof(Handler)} = {Handler}, " +
                $"{nameof(Name)} = {Name}";
        }
    }
}