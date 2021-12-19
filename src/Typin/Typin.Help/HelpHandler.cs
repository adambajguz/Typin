namespace Typin.Help
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using PackSite.Library.Pipelining;
    using Typin.Console;
    using Typin.Features;
    using Typin.Schemas;

    /// <summary>
    /// A middleware that handles help.
    /// </summary>
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
            IBinderFeature binder = args.Binder;
            CommandSchema commandSchema = args.Command.Schema;

            // Help option
            if (binder.UnboundedInput.Options.Any(x => x.Alias is "h" or "help"))
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
