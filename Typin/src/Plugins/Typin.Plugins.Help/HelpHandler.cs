namespace Typin.Plugins.Help
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using PackSite.Library.Pipelining;
    using Typin;
    using Typin.Commands.Features;
    using Typin.Console;
    using Typin.Features;

    /// <summary>
    /// A middleware that handles help.
    /// </summary>
    public sealed class HelpHandler : IMiddleware, IDisposable
    {
        private readonly IHelpWriter _helpTextWriter;
        private readonly IConsole _console;

        private ApplicationMetadata _metadata;
        private readonly IDisposable _metadataMonitor;

        private HelpHandlerOptions _helpHandlerOptions;
        private readonly IDisposable _helpHandlerOptionsMonitor;

        /// <summary>
        /// Initializes a new instance of <see cref="HelpHandler"/>.
        /// </summary>
        public HelpHandler(IHelpWriter helpTextWriter,
                           IConsole console,
                           IOptionsMonitor<ApplicationMetadata> metadata,
                           IOptionsMonitor<HelpHandlerOptions> helpHandlerOptions)
        {
            _helpTextWriter = helpTextWriter;
            _console = console;

            _metadata = metadata.CurrentValue;
            _metadataMonitor = metadata.OnChange((value, namedOptions) =>
            {
                _metadata = value;
            });

            _helpHandlerOptions = helpHandlerOptions.CurrentValue;
            _helpHandlerOptionsMonitor = helpHandlerOptions.OnChange((value, namedOptions) =>
            {
                _helpHandlerOptions = value;
            });
        }

        /// <inheritdoc/>
        public async ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            // Get input and command schema from context
            IBinderFeature binder = args.Binder;
            ICommandFeature commandFeature = args.Features.Get<ICommandFeature>() ??
                throw new InvalidOperationException($"{nameof(ICommandFeature)} has not been configured for this application or call.");

            // Help option
            if (_helpHandlerOptions.HelpEnabled &&
                binder.UnboundedInput.Options.Any(x => x.Alias is "h" or "help"))
            {
                var commandDefaultValues = commandFeature.DefaultValues ?? throw new NullReferenceException($"{nameof(ICommandFeature.DefaultValues)} must be set in {nameof(ICommandFeature)}.");
                _helpTextWriter.Write(commandFeature.Schema, commandDefaultValues);

                args.Output.ExitCode ??= ExitCode.Success;
                return;
            }

            // Version option
            if (_helpHandlerOptions.VersionEnabled &&
                commandFeature.Schema.IsDefault && binder.UnboundedInput.Options.Any(x => x.Alias is "version")) //TODO: add global options mechanism that binds data to class other than ICommand
            {
                _console.Output.WriteLine(_metadata.VersionText);

                args.Output.ExitCode ??= ExitCode.Success;
                return;
            }

            await next();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _metadataMonitor.Dispose();
            _helpHandlerOptionsMonitor.Dispose();
        }
    }
}
