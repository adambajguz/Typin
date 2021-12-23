namespace Typin.Features
{
    using System.Collections.Generic;
    using Typin;
    using Typin.Models.Schemas;
    using Typin.Schemas;

    /// <summary>
    /// <see cref="ICommandFeature"/> implementation.
    /// </summary>
    internal sealed class CommandFeature : ICommandFeature
    {
        /// <inheritdoc/>
        public ICommandSchema Schema { get; }

        /// <inheritdoc/>
        public ICommand Instance { get; }

        /// <inheritdoc/>
        public IReadOnlyDictionary<IArgumentSchema, object?> DefaultValues { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="CommandFeature"/>.
        /// </summary>
        public CommandFeature(ICommandSchema schema,
                              ICommand instance,
                              IReadOnlyDictionary<IArgumentSchema, object?> defaultValues)
        {
            Schema = schema;
            Instance = instance;
            DefaultValues = defaultValues;
        }

        /// <inheritdoc/>
        public override string? ToString()
        {
            return base.ToString() +
                " | " +
                $"{nameof(Schema)}{nameof(ICommandSchema.Name)} = {Schema.Name}, " +
                $"{nameof(Schema)}{nameof(ICommandSchema.Type)} = {Schema.Type}";
        }
    }
}
