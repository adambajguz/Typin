namespace Typin.Help
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using PackSite.Library.Pipelining;
    using Typin.Console;
    using Typin.Input;
    using Typin.Schemas;

    public sealed class HelpHandler : IMiddleware
    {
        private readonly IHelpWriter _helpTextWriter;
        private readonly IConsole _console;
        private readonly IOptionsMonitor<ApplicationMetadata> _metadata;

        /// <summary>
        /// Initializes a new instance of <see cref="HelpHandler"/>.
        /// </summary>
        public HelpHandler(IHelpWriter helpTextWriter,
                           IConsole console,
                           IOptionsMonitor<ApplicationMetadata> metadata)
        {
            _helpTextWriter = helpTextWriter;
            _console = console;
            _metadata = metadata;
        }

        /// <inheritdoc/>
        public async ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            // Get input and command schema from context
            ParsedCommandInput input = args.Input.Parsed ?? throw new NullReferenceException($"{nameof(CliContext.Input.Parsed)} must be set in {nameof(CliContext)}.");
            CommandSchema commandSchema = args.Command.Schema;

            // Help option
            if ((commandSchema.IsHelpOptionAvailable && input.IsHelpOptionSpecified) ||
                (commandSchema.IsDefault && input.IsDefaultCommandOrEmpty))
            {
                var commandDefaultValues = args.Command.DefaultValues ?? throw new NullReferenceException($"{nameof(CliContext.Command.DefaultValues)} must be set in {nameof(CliContext)}.");
                _helpTextWriter.Write(commandSchema, commandDefaultValues);

                args.Output.ExitCode ??= ExitCode.Success;
                return;
            }

            await next();
        }
    }
}
