namespace Typin.Commands.Features
{
    using System.Collections.Generic;
    using Typin.Commands;
    using Typin.Commands.Schemas;
    using Typin.Models.Schemas;

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
        public ICommandHandler HandlerInstance { get; }

        /// <inheritdoc/>
        public IReadOnlyDictionary<IArgumentSchema, object?> DefaultValues { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="CommandFeature"/>.
        /// </summary>
        public CommandFeature(ICommandSchema schema,
                              ICommand instance,
                              ICommandHandler commandHandler,
                              IReadOnlyDictionary<IArgumentSchema, object?> defaultValues)
        {
            Schema = schema;
            Instance = instance;
            HandlerInstance = commandHandler;
            DefaultValues = defaultValues;
        }

        /// <inheritdoc/>
        public override string? ToString()
        {
            return base.ToString() +
                " | " +
                $"{nameof(Schema)} = {Schema}";
        }
    }
}
