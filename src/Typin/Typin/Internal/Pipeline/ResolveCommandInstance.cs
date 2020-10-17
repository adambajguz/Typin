namespace Typin.Internal.Pipeline
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Typin;
    using Typin.Internal;
    using Typin.Schemas;

    internal sealed class ResolveCommandInstance : IMiddleware
    {
        private readonly IServiceProvider _serviceProvider;

        public ResolveCommandInstance(IServiceProvider serviceProvider)
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
            // Get command schema from context
            CommandSchema commandSchema = context.CommandSchema;

            // Get command instance (also used in help text)
            ICommand instance = GetCommandInstance(commandSchema);
            context.Command = instance;

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
    }
}
