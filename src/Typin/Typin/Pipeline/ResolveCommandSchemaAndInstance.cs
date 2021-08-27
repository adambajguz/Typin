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
    using Typin.DynamicCommands;
    using Typin.Input;
    using Typin.Internal;
    using Typin.Internal.DynamicCommands;
    using Typin.Schemas;

    /// <summary>
    /// Resolves command schema and instance.
    /// </summary>
    public sealed class ResolveCommandSchemaAndInstance : IMiddleware
    {
        private static Action<IDynamicCommand, IArgumentCollection>? _dynamicCommandArgumentCollectionSetter;

        private readonly IRootSchemaAccessor _rootSchemaAccessor;
        private readonly IServiceProvider _serviceProvider;

        private readonly ConcurrentDictionary<Type, ObjectFactory> _commandFactoryCache = new();

        /// <summary>
        /// Initializes a new instance of <see cref="ResolveCommandSchemaAndInstance"/>.
        /// </summary>
        public ResolveCommandSchemaAndInstance(IRootSchemaAccessor rootSchemaAccessor, IServiceProvider serviceProvider)
        {
            _rootSchemaAccessor = rootSchemaAccessor;
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc/>
        public async ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            args.ExitCode ??= Execute(args);

            await next();
        }

        private int? Execute(CliContext context)
        {
            CommandInput input = context.Input ?? throw new NullReferenceException($"{nameof(CliContext.PipelinedDirectives)} must be set in {nameof(CliContext)}.");

            // Try to get the command matching the input or fallback to default
            CommandSchema commandSchema = _rootSchemaAccessor.RootSchema.TryFindCommand(input.CommandName) ?? StubDefaultCommand.Schema;

            // TODO: is the problem below still valid?
            // TODO: is it poossible to overcome this (related to [!]) limitation of new mode system
            // Forbid to execute real default command in interactive mode without [!] directive.
            //if (!(commandSchema.IsHelpOptionAvailable && input.IsHelpOptionSpecified) &&
            //    _applicationLifetime.CurrentModeType == typeof(InteractiveMode) &&
            //    commandSchema.IsDefault && !hasDefaultDirective)
            //{
            //    commandSchema = StubDefaultCommand.Schema;
            //}

            // Update CommandSchema
            context.CommandSchema = commandSchema;

            // Get command instance (default values are used in help so we need command instance)
            ICommand instance = GetCommandInstance(commandSchema);
            context.Command = instance;

            if (commandSchema.IsDynamic && instance is IDynamicCommand dynamicCommandInstance)
            {
                _dynamicCommandArgumentCollectionSetter ??= GetDynamicArgumentsSetter();
                _dynamicCommandArgumentCollectionSetter.Invoke(dynamicCommandInstance, new ArgumentCollection());
            }

            // To avoid instantiating the command twice, we need to get default values
            // before the arguments are bound to the properties
            IReadOnlyDictionary<ArgumentSchema, object?> defaultValues = commandSchema.GetArgumentValues(instance);
            context.CommandDefaultValues = defaultValues;

            return null;
        }

        private ICommand GetCommandInstance(CommandSchema command)
        {
            if (command == StubDefaultCommand.Schema)
            {
                return new StubDefaultCommand();
            }

            ObjectFactory factory = _commandFactoryCache.GetOrAdd(command.Type, (key) =>
            {
                return ActivatorUtilities.CreateFactory(key, Array.Empty<Type>());
            });

            return (ICommand)factory(_serviceProvider, null);
        }

        private static Action<IDynamicCommand, IArgumentCollection> GetDynamicArgumentsSetter()
        {
            MethodInfo methodInfo = typeof(IDynamicCommand).GetProperty(nameof(IDynamicCommand.Arguments))!.GetSetMethod(true)!;
            var @delegate = (Action<IDynamicCommand, IArgumentCollection>)Delegate.CreateDelegate(typeof(Action<IDynamicCommand, IArgumentCollection>), methodInfo);

            return @delegate;
        }
    }
}
