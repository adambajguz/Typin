namespace Typin.Features
{
    using System.Collections.Generic;
    using Typin;
    using Typin.Schemas;

    /// <summary>
    /// <see cref="ICommandFeature"/> implementation.
    /// </summary>
    internal sealed class CommandFeature : ICommandFeature
    {
        /// <inheritdoc/>
        public CommandSchema Schema { get; }

        /// <inheritdoc/>
        public ICommand Instance { get; }

        /// <inheritdoc/>
        public IReadOnlyDictionary<ArgumentSchema, object?> DefaultValues { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="CommandFeature"/>.
        /// </summary>
        public CommandFeature(CommandSchema schema,
                              ICommand instance,
                              IReadOnlyDictionary<ArgumentSchema, object?> defaultValues)
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
                $"{nameof(Schema)}{nameof(CommandSchema.Name)} = {Schema.Name}, " +
                $"{nameof(Schema)}{nameof(CommandSchema.Type)} = {Schema.Type}";
        }
    }
}
