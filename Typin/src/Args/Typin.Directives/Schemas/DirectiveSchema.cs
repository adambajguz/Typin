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
        public IReadOnlyAliasCollection Aliases { get; }

        /// <inheritdoc/>
        public bool IsDefault { get; }

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
        public DirectiveSchema(IReadOnlyAliasCollection aliases,
                               string? description,
                               IModelSchema model,
                               Type handler,
                               IExtensionsCollection extensions)
        {
            Aliases = new AliasCollection(aliases ?? throw new ArgumentNullException(nameof(aliases)));
            IsDefault = Aliases.Contains(string.Empty);
            Description = description;
            Model = model ?? throw new ArgumentNullException(nameof(model));
            Handler = handler ?? throw new ArgumentNullException(nameof(handler));
            Extensions = extensions ?? throw new ArgumentNullException(nameof(extensions));
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return base.ToString() +
                " | " +
                $"{nameof(Model)}.{nameof(IModelSchema.Type)} = {Model.Type}, " +
                $"{nameof(Handler)} = {Handler}, " +
                $"{nameof(Aliases)} = {Aliases}, " +
                $"{nameof(IsDefault)} = {IsDefault}";
        }
    }
}