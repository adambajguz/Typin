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

    /// <summary>
    /// A middleware that handles version information.
    /// </summary>
    public sealed class VersionHandler : IMiddleware
    {
        private readonly IHelpWriter _helpTextWriter;
        private readonly IConsole _console;
        private readonly IOptionsMonitor<ApplicationMetadata> _metadata;

        /// <summary>
        /// Initializes a new instance of <see cref="HelpHandler"/>.
        /// </summary>
        public VersionHandler(IHelpWriter helpTextWriter,
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

            // Version option
            if (commandSchema.IsVersionOptionAvailable && input.IsVersionOptionSpecified)
            {
                _console.Output.WriteLine(_metadata.CurrentValue.VersionText);

                args.Output.ExitCode ??= ExitCode.Success;
                return;
            }

            await next();
        }
    }
}
