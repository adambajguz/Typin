namespace Typin.Internal.Pipeline
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Typin;
    using Typin.DynamicCommands;
    using Typin.Input;
    using Typin.Internal;
    using Typin.Schemas;

    internal sealed class ResolveCommandSchemaAndInstance : IMiddleware
    {
        private static Action<IDynamicCommand, IDynamicArgumentCollection>? _dynamicCommandArguemtnCollectionSetter;

        private readonly IServiceProvider _serviceProvider;

        public ResolveCommandSchemaAndInstance(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task HandleAsync(ICliContext context, CommandPipelineHandlerDelegate next, CancellationToken cancellationToken)
        {
            context.ExitCode ??= Execute((CliContext)context);

            await next();
        }

        private int? Execute(CliContext context)
        {
            RootSchema root = context.RootSchema;
            CommandInput input = context.Input;

            // Try to get the command matching the input or fallback to default
            CommandSchema commandSchema = root.TryFindCommand(input.CommandName) ?? StubDefaultCommand.Schema;

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
                _dynamicCommandArguemtnCollectionSetter ??= GetDynamicArgumentsSetter();
                _dynamicCommandArguemtnCollectionSetter.Invoke(dynamicCommandInstance, new DynamicArgumentCollection());
            }

            // To avoid instantiating the command twice, we need to get default values
            // before the arguments are bound to the properties
            IReadOnlyDictionary<ArgumentSchema, object?> defaultValues = commandSchema.GetArgumentValues(instance);
            context.CommandDefaultValues = defaultValues;

            return null;
        }

        private ICommand GetCommandInstance(CommandSchema command)
        {
            return command != StubDefaultCommand.Schema ? (ICommand)_serviceProvider.GetRequiredService(command.Type) : new StubDefaultCommand();
        }

        private static Action<IDynamicCommand, IDynamicArgumentCollection> GetDynamicArgumentsSetter()
        {
            MethodInfo methodInfo = typeof(IDynamicCommand).GetProperty(nameof(IDynamicCommand.Arguments))!.GetSetMethod(true)!;
            var @delegate = (Action<IDynamicCommand, IDynamicArgumentCollection>)Delegate.CreateDelegate(typeof(Action<IDynamicCommand, IDynamicArgumentCollection>), methodInfo);

            return @delegate;
        }
    }
}
