namespace Typin.Internal.Pipeline
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Input;
    using Typin.Schemas;

    internal sealed class HandleVersionOption : IMiddleware
    {
        public HandleVersionOption()
        {

        }

        public async Task HandleAsync(ICliContext context, CommandPipelineHandlerDelegate next, CancellationToken cancellationToken)
        {
            context.ExitCode ??= Execute((CliContext)context);

            await next();
        }

        private int? Execute(CliContext context)
        {
            CommandInput input = context.Input;

            // Get command schema from context
            CommandSchema commandSchema = context.CommandSchema;

            // Version option
            if (commandSchema.IsVersionOptionAvailable && input.IsVersionOptionSpecified)
            {
                context.Console.Output.WriteLine(context.Metadata.VersionText);

                return ExitCodes.Success;
            }

            return null;
        }
    }
}
