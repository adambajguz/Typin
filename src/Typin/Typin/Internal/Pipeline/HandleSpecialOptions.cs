namespace Typin.Internal.Pipeline
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.HelpWriter;
    using Typin.Input;
    using Typin.Schemas;

    internal sealed class HandleSpecialOptions : IMiddleware
    {
        private readonly IHelpWriter _helpTextWriter;

        public HandleSpecialOptions(IHelpWriter helpTextWriter)
        {
            _helpTextWriter = helpTextWriter;
        }

        public async Task HandleAsync(ICliContext context, CommandPipelineHandlerDelegate next, CancellationToken cancellationToken)
        {
            // Get input and command schema from context
            CommandInput input = context.Input;
            CommandSchema commandSchema = context.CommandSchema;

            // Version option
            if (commandSchema.IsVersionOptionAvailable && input.IsVersionOptionSpecified)
            {
                context.Console.Output.WriteLine(context.Metadata.VersionText);

                context.ExitCode ??= ExitCodes.Success;
                return;
            }

            // Help option
            if ((commandSchema.IsHelpOptionAvailable && input.IsHelpOptionSpecified) ||
                (commandSchema == StubDefaultCommand.Schema && input.IsDefaultCommandOrEmpty))
            {
                _helpTextWriter.Write(commandSchema, context.CommandDefaultValues);

                context.ExitCode ??= ExitCodes.Success;
                return;
            }

            await next();
        }
    }
}
