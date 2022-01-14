namespace Typin.Commands.Pipeline
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using PackSite.Library.Pipelining;
    using Typin.Commands;
    using Typin.Commands.Collections;
    using Typin.Commands.Features;
    using Typin.Commands.Schemas;
    using Typin.Features.Binding;
    using Typin.Features.Input;
    using Typin.Features.Input.Tokens;
    using Typin.Models.Schemas;

    /// <summary>
    /// Resolves command schema and instance.
    /// </summary>
    public sealed class ResolveCommand : IMiddleware //TODO: as directive
    {
        private readonly ICommandSchemaCollection _commandSchemas;

        private readonly ConcurrentDictionary<Type, ObjectFactory> _commandFactoryCache = new();

        /// <summary>
        /// Initializes a new instance of <see cref="ResolveCommand"/>.
        /// </summary>
        public ResolveCommand(ICommandSchemaCollection commandSchemas)
        {
            _commandSchemas = commandSchemas;
        }

        /// <inheritdoc/>
        public async ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            Execute(args);

            await next();
        }

        private void Execute(CliContext context)
        {
            IServiceProvider serviceProvider = context.Services;

            IUnboundedDirectiveToken? possibleCommandDirective = context.Binder.UnboundedTokens.LastOrDefault();

            if (possibleCommandDirective is null ||
                !(possibleCommandDirective.MatchesAlias(string.Empty) ||
                  possibleCommandDirective.MatchesAlias("exec"))) //TODO: make dynamic
            {
                return;
            }

            // Try to get the command matching the input or fallback to default
            ICommandSchema schema = BindInputToCommandSchema(possibleCommandDirective.Children);

            // Get command instance (default values are used in help so we need command instance)

            ICommand instance = GetCommandInstance(serviceProvider, schema);
            ICommandHandler handlerInstance = GetCommandHandlerInstance(serviceProvider, instance, schema);

            // To avoid instantiating the command twice, we need to get default values
            // before the arguments are bound to the properties
            IReadOnlyDictionary<IArgumentSchema, object?> defaultValues = schema.Model.GetArgumentValues(instance);

            context.Features.Set<ICommandFeature>(new CommandFeature(schema, instance, handlerInstance, defaultValues));
            context.Binder.TryAdd(new BindableModel(possibleCommandDirective.Id, schema.Model, instance));

            return;
        }

        private ICommandSchema BindInputToCommandSchema(IUnboundedTokenCollection? input)
        {
            TokenGroup<IValueToken>? tokenGroup = input?.Get<IValueToken>();

            if (tokenGroup is null)
            {
                return _commandSchemas.Get(string.Empty) ??
                    throw new InvalidOperationException($"Unknown command ''.");
            }

            ICommandSchema? schema = null;

            List<string> buffer = new();
            int lastIndex = -1;

            // We need to look ahead to see if we can match as many consecutive arguments to a command name as possible
            for (int i = 0; i < tokenGroup.Tokens.Count; i++)
            {
                string argument = tokenGroup.Tokens[i].Value;
                buffer.Add(argument);

                string potentialCommandName = string.Join(' ', buffer);

                if (_commandSchemas.Get(potentialCommandName) is ICommandSchema potentialCommandSchema)
                {
                    lastIndex = i;
                    schema = potentialCommandSchema;
                }
            }

            if (schema is null)
            {
                throw new InvalidOperationException($"Unknown command '{buffer}'.");
            }

            for (int r = 0; r < lastIndex; r++)
            {
                tokenGroup.Tokens.RemoveAt(0);
            }

            return schema;
        }

        private ICommand GetCommandInstance(IServiceProvider serviceProvider, ICommandSchema schema)
        {
            ObjectFactory factory = _commandFactoryCache.GetOrAdd(schema.Model.Type, (key) =>
            {
                return ActivatorUtilities.CreateFactory(key, Array.Empty<Type>());
            });

            return (ICommand)factory(serviceProvider, null);
        }

        private ICommandHandler GetCommandHandlerInstance(IServiceProvider serviceProvider, ICommand instance, ICommandSchema schema)
        {
            if (schema.Handler == schema.Model.Type)
            {
                return (ICommandHandler)instance;
            }

            ObjectFactory factory = _commandFactoryCache.GetOrAdd(schema.Handler, (key) =>
            {
                return ActivatorUtilities.CreateFactory(key, Array.Empty<Type>());
            });

            return (ICommandHandler)factory(serviceProvider, null);
        }
    }
}
