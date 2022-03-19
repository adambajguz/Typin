namespace Typin.Commands.Resolvers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Commands.Collections;
    using Typin.Commands.Schemas;
    using Typin.Hosting.Components;

    /// <summary>
    /// Default implementation of <see cref="ICommandSchemaProvider"/>
    /// </summary>
    internal sealed class CommandSchemaProvider : ICommandSchemaProvider
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IComponents<ICommand> _commands;

        /// <inheritdoc/>
        public ICommandSchemaCollection Schemas { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="CommandSchemaProvider"/>.
        /// </summary>
        public CommandSchemaProvider(IServiceProvider serviceProvider,
                                     ICommandSchemaCollection schemas,
                                     IComponents<ICommand> commands)
        {
            _serviceProvider = serviceProvider;
            _commands = commands;

            Schemas = schemas;
        }

        /// <inheritdoc/>
        public async Task ReloadAsync(CancellationToken cancellationToken = default)
        {
            IReadOnlyCollection<Type>? CommandsTypes = _commands.Types;

            if (CommandsTypes is null or { Count: 0 })
            {
                return;
            }

            foreach (Type type in CommandsTypes)
            {
                Type commandSchemaResolverType = typeof(ICommandSchemaResolver<>).MakeGenericType(type);

                ICommandSchemaResolver commandSchemaResolver = (ICommandSchemaResolver)_serviceProvider.GetRequiredService(commandSchemaResolverType);
                ICommandSchema schema = await commandSchemaResolver.ResolveAsync(cancellationToken);

                Schemas.Set(schema); //TODO: better validation, e.g. name duplicates
            }
        }
    }
}
