namespace Typin.Help
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using PackSite.Library.Pipelining;
    using Typin.Console;
    using Typin.Features;
    using Typin.Schemas;

    /// <summary>
    /// A middleware that handles version information.
    /// </summary>
    public sealed class VersionHandler : IMiddleware
    {
        private readonly IConsole _console;
        private readonly IOptionsMonitor<ApplicationMetadata> _metadata;

        /// <summary>
        /// Initializes a new instance of <see cref="HelpHandler"/>.
        /// </summary>
        public VersionHandler(IConsole console,
                              IOptionsMonitor<ApplicationMetadata> metadata)
        {
            _console = console;
            _metadata = metadata;
        }

        /// <inheritdoc/>
        public async ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            // Get input and command schema from context
            IBinderFeature binder = args.Binder;
            CommandSchema commandSchema = args.Command.Schema;

            // Version option
            if (commandSchema.IsVersionOptionAvailable && binder.UnboundedInput.Options.Any(x => x.Alias is "version"))
            {
                _console.Output.WriteLine(_metadata.CurrentValue.VersionText);

                args.Output.ExitCode ??= ExitCode.Success;
                return;
            }

            await next();
        }
    }
}
