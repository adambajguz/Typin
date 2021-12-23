namespace Typin.Pipeline
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using PackSite.Library.Pipelining;
    using Typin;
    using Typin.Exceptions.ArgumentBinding;
    using Typin.Features;
    using Typin.Features.Binding;
    using Typin.Features.Input;
    using Typin.Models.Collections;
    using Typin.Models.Schemas;
    using Typin.Schemas;

    /// <summary>
    /// Resolves command schema and instance.
    /// </summary>
    public sealed class ResolveCommand : IMiddleware
    {
        private static Action<ICommandTemplate, IArgumentCollection>? _dynamicCommandArgumentCollectionSetter;

        private readonly IRootSchemaAccessor _rootSchemaAccessor;
        private readonly IServiceProvider _serviceProvider;

        private readonly ConcurrentDictionary<Type, ObjectFactory> _commandFactoryCache = new();

        /// <summary>
        /// Initializes a new instance of <see cref="ResolveCommand"/>.
        /// </summary>
        public ResolveCommand(IRootSchemaAccessor rootSchemaAccessor, IServiceProvider serviceProvider)
        {
            _rootSchemaAccessor = rootSchemaAccessor;
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc/>
        public async ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            args.Output.ExitCode ??= Execute(args);

            await next();
        }

        private int? Execute(CliContext context)
        {
            ParsedInput input = context.Input.Parsed ?? throw new NullReferenceException($"{nameof(CliContext.Input.Parsed)} must be set in {nameof(CliContext)}.");

            // Try to get the command matching the input or fallback to default
            ICommandSchema schema = _rootSchemaAccessor.RootSchema.TryFindCommand(input.CommandName)
                ?? throw new UnknownCommandException(input.CommandName);

            // TODO: is the problem below still valid?
            // TODO: is it poossible to overcome this (related to [!]) limitation of new mode system
            // Forbid to execute real default command in interactive mode without [!] directive.
            //if (!(commandSchema.IsHelpOptionAvailable && input.IsHelpOptionSpecified) &&
            //    _applicationLifetime.CurrentModeType == typeof(InteractiveMode) &&
            //    commandSchema.IsDefault && !hasDefaultDirective)
            //{
            //    commandSchema = StubDefaultCommand.Schema;
            //}

            // Get command instance (default values are used in help so we need command instance)

            ICommand instance = GetCommandInstance(schema);
            ICommandHandler handlerInstance = GetCommandHandlerInstance(instance, schema);

            if (schema.IsDynamic && instance is ICommandTemplate dynamicCommandInstance)
            {
                _dynamicCommandArgumentCollectionSetter ??= GetDynamicArgumentsSetter();
                _dynamicCommandArgumentCollectionSetter.Invoke(dynamicCommandInstance, new ArgumentCollection());
            }

            // To avoid instantiating the command twice, we need to get default values
            // before the arguments are bound to the properties
            IReadOnlyDictionary<IArgumentSchema, object?> defaultValues = schema.Model.GetArgumentValues(instance);

            context.Features.Set<ICommandFeature>(new CommandFeature(schema, instance, handlerInstance, defaultValues));
            context.Binder.TryAdd(new BindableModel(schema.Model, instance));

            return null;
        }

        private ICommand GetCommandInstance(ICommandSchema schema)
        {
            ObjectFactory factory = _commandFactoryCache.GetOrAdd(schema.Model.Type, (key) =>
            {
                return ActivatorUtilities.CreateFactory(key, Array.Empty<Type>());
            });

            return (ICommand)factory(_serviceProvider, null);
        }

        private ICommandHandler GetCommandHandlerInstance(ICommand instance, ICommandSchema schema)
        {
            if (schema.Handler == schema.Model.Type)
            {
                return (ICommandHandler)instance;
            }

            ObjectFactory factory = _commandFactoryCache.GetOrAdd(schema.Handler, (key) =>
            {
                return ActivatorUtilities.CreateFactory(key, Array.Empty<Type>());
            });

            return (ICommandHandler)factory(_serviceProvider, null);
        }

        private static Action<ICommandTemplate, IArgumentCollection> GetDynamicArgumentsSetter()
        {
            MethodInfo methodInfo = typeof(ICommandTemplate).GetProperty(nameof(ICommandTemplate.Arguments))!.GetSetMethod(true)!;
            var @delegate = (Action<ICommandTemplate, IArgumentCollection>)Delegate.CreateDelegate(typeof(Action<ICommandTemplate, IArgumentCollection>), methodInfo);

            return @delegate;
        }
    }
}
