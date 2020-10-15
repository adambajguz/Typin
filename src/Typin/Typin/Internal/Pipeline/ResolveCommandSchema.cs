namespace Typin.Internal.Pipeline
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Input;
    using Typin.Internal;
    using Typin.Schemas;

    internal sealed class ResolveCommandSchema : IMiddleware
    {
        public ResolveCommandSchema()
        {

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
            CommandSchema command = root.TryFindCommand(input.CommandName, hasDefaultDirective) ?? StubDefaultCommand.Schema;

            // Forbid to execute real default command in interactive mode without [!] directive.
            if (!(command.IsHelpOptionAvailable && input.IsHelpOptionSpecified) &&
                context.IsInteractiveMode && command.IsDefault && !hasDefaultDirective)
            {
                command = StubDefaultCommand.Schema;
            }

            // Update CommandSchema
            context.CommandSchema = command;

            return null;
        }
    }
}
