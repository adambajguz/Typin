namespace Typin.Pipeline
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using PackSite.Library.Pipelining;
    using Typin;
    using Typin.Console;
    using Typin.Help;
    using Typin.Input;
    using Typin.Internal;
    using Typin.Schemas;

    /// <summary>
    /// Handles special options.
    /// </summary>
    public sealed class HandleSpecialOptions : IMiddleware
    {
        private readonly IHelpWriter _helpTextWriter;
        private readonly IConsole _console;
        private readonly IOptionsMonitor<ApplicationMetadata> _metadata;

        /// <summary>
        /// Initializes a new instance of <see cref="HandleSpecialOptions"/>.
        /// </summary>
        public HandleSpecialOptions(IHelpWriter helpTextWriter, IConsole console, IOptionsMonitor<ApplicationMetadata> metadata)
        {
            _helpTextWriter = helpTextWriter;
            _console = console;
            _metadata = metadata;
        }

        /// <inheritdoc/>
        public async ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            // Get input and command schema from context
            CommandInput input = args.Input ?? throw new NullReferenceException($"{nameof(CliContext.Input)} must be set in {nameof(CliContext)}.");
            CommandSchema commandSchema = args.CommandSchema ?? throw new NullReferenceException($"{nameof(CliContext.CommandSchema)} must be set in {nameof(CliContext)}.");

            // Version option
            if (commandSchema.IsVersionOptionAvailable && input.IsVersionOptionSpecified)
            {
                _console.Output.WriteLine(_metadata.CurrentValue.VersionText);

                args.ExitCode ??= ExitCodes.Success;
                return;
            }

            // Help option
            if ((commandSchema.IsHelpOptionAvailable && input.IsHelpOptionSpecified) ||
                (commandSchema == StubDefaultCommand.Schema && input.IsDefaultCommandOrEmpty))
            {
                var commandDefaultValues = args.CommandDefaultValues ?? throw new NullReferenceException($"{nameof(CliContext.CommandDefaultValues)} must be set in {nameof(CliContext)}.");
                _helpTextWriter.Write(commandSchema, commandDefaultValues);

                args.ExitCode ??= ExitCodes.Success;
                return;
            }

            await next();
        }
    }
}
