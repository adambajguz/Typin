namespace Typin.Commands.Resolvers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Commands.Builders;
    using Typin.Commands.Schemas;
    using Typin.Models.Collections;
    using Typin.Models.Schemas;

    /// <summary>
    /// Command schema resolver.
    /// </summary>
    internal sealed class CommandSchemaResolver<TCommand> : ICommandSchemaResolver<TCommand>
        where TCommand : class, ICommand
    {
        private readonly IModelSchemaCollection _modelSchemas;
        private readonly IEnumerable<IConfigureCommand<TCommand>> _configurators;

        /// <summary>
        /// Initializes a new instance of <see cref="CommandSchemaProvider"/>.
        /// </summary>
        public CommandSchemaResolver(IEnumerable<IConfigureCommand<TCommand>> configurators,
                                     IModelSchemaCollection modelSchemas)
        {
            _configurators = configurators;
            _modelSchemas = modelSchemas;
        }

        /// <inheritdoc/>
        public async Task<ICommandSchema> ResolveAsync(CancellationToken cancellationToken = default)
        {
            IModelSchema commandModelSchema = _modelSchemas[typeof(TCommand)] ??
                throw new InvalidOperationException($"Cannot resolve '{typeof(ICommandSchema)}' for '{typeof(TCommand)}' because its '{nameof(IModelSchema)}' was not found.");

            ICommandBuilder<TCommand> builder = new CommandBuilder<TCommand>(commandModelSchema);

            foreach (IConfigureCommand<TCommand> configurator in _configurators)
            {
                await configurator.ConfigureAsync(builder, cancellationToken);
            }

            ICommandSchema schema = builder.Build();

            return schema;
        }
    }
}
