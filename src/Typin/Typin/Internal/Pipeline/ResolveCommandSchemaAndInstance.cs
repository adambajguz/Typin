namespace Typin.Internal.Pipeline
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Typin;
    using Typin.Directives;
    using Typin.Input;
    using Typin.Internal;
    using Typin.Schemas;

    internal sealed class ResolveCommandSchemaAndInstance : IMiddleware
    {
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
            bool hasDefaultDirective = input.HasDirective(BuiltInDirectives.Default);
            CommandSchema commandSchema = root.TryFindCommand(input.CommandName, hasDefaultDirective) ?? StubDefaultCommand.Schema;

            // Forbid to execute real default command in interactive mode without [!] directive.
            //if (!(command.IsHelpOptionAvailable && input.IsHelpOptionSpecified) &&
            //    context.ModeSwitcher.Current == CliModes.Interactive &&
            //    command.IsDefault && !hasDefaultDirective)
            //{
            //    command = StubDefaultCommand.Schema;
            //}

            // Update CommandSchema
            context.CommandSchema = commandSchema;

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
