namespace Typin.Internal.Pipeline
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Input;
    using Typin.Internal;
    using Typin.Schemas;

    internal sealed class HandleHelpOption : IMiddleware
    {
        public HandleHelpOption()
        {

        }

        public async Task HandleAsync(ICliContext context, CommandPipelineHandlerDelegate next, CancellationToken cancellationToken)
        {
            context.ExitCode ??= Execute((CliContext)context);

            await next();
        }

        private int? Execute(CliContext context)
        {
            // Get input and command schema from context
            CommandInput input = context.Input;
            CommandSchema commandSchema = context.CommandSchema;

            // Help option
            if ((commandSchema.IsHelpOptionAvailable && input.IsHelpOptionSpecified) ||
                (commandSchema == StubDefaultCommand.Schema && !input.Parameters.Any() && !input.Options.Any()))
            {
                HelpTextWriter helpTextWriter = new HelpTextWriter(context);
                helpTextWriter.Write(context.RootSchema, commandSchema, context.CommandDefaultValues); //TODO: add directives help?

                return ExitCodes.Success;
            }

            return null;
        }
    }
}
