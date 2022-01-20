namespace Typin.Commands.Directives
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using PackSite.Library.Pipelining;
    using Typin.Commands;
    using Typin.Commands.Collections;
    using Typin.Commands.Features;
    using Typin.Commands.Schemas;
    using Typin.Directives;
    using Typin.Directives.Builders;
    using Typin.Features;
    using Typin.Features.Binding;
    using Typin.Features.Input;
    using Typin.Features.Input.Tokens;
    using Typin.Models;
    using Typin.Models.Builders;
    using Typin.Models.Schemas;

    /// <summary>
    /// A directive that executes a command.
    /// </summary>
    public sealed class ExecuteCommandDirective : IDirective
    {
        /// <summary>
        /// <see cref="ExecuteCommandDirective"/> handler.
        /// </summary>
        public sealed class Handler : IDirectiveHandler<ExecuteCommandDirective>
        {
            private static readonly ConcurrentDictionary<Type, ObjectFactory> _commandFactoryCache = new();

            private readonly ICommandSchemaCollection _commandSchemas;

            /// <summary>
            /// Initializes a new instance of <see cref="ExecuteCommandDirective"/>.
            /// </summary>
            public Handler(ICommandSchemaCollection commandSchemas)
            {
                _commandSchemas = commandSchemas;
            }

            /// <inheritdoc/>
            public async ValueTask ExecuteAsync(DirectiveArgs<ExecuteCommandDirective> args, StepDelegate next, CancellationToken cancellationToken = default)
            {
                ResolveCommand(args);

                await ExecuteCommand(args, cancellationToken);
            }

            private static async Task ExecuteCommand(CliContext context, CancellationToken cancellationToken)
            {
                // Get command instance from context
                ICommandFeature commandFeature = context.Features.Get<ICommandFeature>() ??
                 throw new InvalidOperationException($"{nameof(ICommandFeature)} has not been configured for this application or call.");

                ICommand instance = commandFeature.Instance;
                ICommandHandler handlerInstance = commandFeature.HandlerInstance;

                // Execute command
                await handlerInstance.ExecuteAsync(instance, cancellationToken);
                context.Output.ExitCode ??= ExitCode.Success;
            }

            private void ResolveCommand(DirectiveArgs<ExecuteCommandDirective> args)
            {
                IServiceProvider serviceProvider = args.Context.Services;
                IBinderFeature binder = args.Context.Binder;

                IUnboundedDirectiveToken commandDirective = binder.UnboundedTokens[args.DirectiveId] ??
                    throw new ApplicationException($"UnboundedTokens with Id '{args.DirectiveId}' were not found.");

                // Try to get the command matching the input or fallback to default
                ICommandSchema schema = BindInputToCommandSchema(commandDirective.Children);

                // Get command instance (default values are used in help so we need command instance)

                ICommand model = GetCommandInstance(serviceProvider, schema);
                ICommandHandler handler = GetCommandHandlerInstance(serviceProvider, model, schema);

                // To avoid instantiating the command twice, we need to get default values
                // before the arguments are bound to the properties
                IReadOnlyDictionary<IArgumentSchema, object?> defaultValues = schema.Model.GetArgumentValues(model);

                CommandFeature instance = new(schema, model, handler, defaultValues);
                args.Context.Features.Set<ICommandFeature>(instance);

                BindableModel bindableModel = new(commandDirective.Id, schema.Model, model);

                binder.Add(bindableModel);

                binder.Bind(serviceProvider);

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

            private static ICommand GetCommandInstance(IServiceProvider serviceProvider, ICommandSchema schema)
            {
                ObjectFactory factory = _commandFactoryCache.GetOrAdd(schema.Model.Type, (key) =>
                {
                    return ActivatorUtilities.CreateFactory(key, Array.Empty<Type>());
                });

                return (ICommand)factory(serviceProvider, null);
            }

            private static ICommandHandler GetCommandHandlerInstance(IServiceProvider serviceProvider, ICommand instance, ICommandSchema schema)
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

        /// <summary>
        /// Configures <see cref="ExecuteCommandDirective"/>.
        /// </summary>
        public sealed class Configure : IConfigureModel<ExecuteCommandDirective>, IConfigureDirective<ExecuteCommandDirective>
        {
            /// <inheritdoc/>
            public ValueTask ConfigureAsync(IModelBuilder<ExecuteCommandDirective> builder, CancellationToken cancellationToken)
            {
                return default;
            }

            /// <inheritdoc/>
            public ValueTask ConfigureAsync(IDirectiveBuilder<ExecuteCommandDirective> builder, CancellationToken cancellationToken)
            {
                builder
                    .ManageAliases(aliases =>
                    {
                        aliases.Add(string.Empty);
                        aliases.Add("exec");
                        aliases.Add("execute");
                    })
                    .UseHandler<Handler>();

                return default;
            }
        }
    }
}
