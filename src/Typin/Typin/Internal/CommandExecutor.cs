namespace Typin.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using PackSite.Library.Pipelining;
    using Typin;
    using Typin.Features;
    using Typin.Utilities;

    /// <summary>
    /// Typin command executor.
    /// </summary>
    internal class CommandExecutor : ICommandExecutor
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ICliModeAccessor _cliModeAccessor;
        private readonly ICliContextAccessor _cliContextAccessor;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="CommandExecutor"/>.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="cliModeAccessor"></param>
        /// <param name="cliContextAccessor"></param>
        /// <param name="logger"></param>
        public CommandExecutor(IServiceProvider serviceProvider,
                               ICliModeAccessor cliModeAccessor,
                               ICliContextAccessor cliContextAccessor,
                               ILogger<CommandExecutor> logger)
        {
            _serviceProvider = serviceProvider;
            _cliModeAccessor = cliModeAccessor;
            _cliContextAccessor = cliContextAccessor;
            _logger = logger;
        }

        /// <summary>
        /// Executes a command.
        /// </summary>
        /// <param name="commandLine"></param>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        public async Task<int> ExecuteAsync(string commandLine, CommandExecutionOptions options = default, CancellationToken cancellationToken = default)
        {
            IEnumerable<string> commandLineArguments = CommandLine.Split(commandLine);

            return await ExecuteAsync(commandLineArguments, options, cancellationToken);
        }

        /// <summary>
        /// Executes a command.
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        public async Task<int> ExecuteAsync(IEnumerable<string> arguments, CommandExecutionOptions options = default, CancellationToken cancellationToken = default)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            using CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            using IServiceScope? serviceScope = options.HasFlag(CommandExecutionOptions.UseCurrentScope) // TODO: use IAsyncServiceProvider
                ? null
                : _serviceProvider.CreateScope();

            IServiceProvider localProvider = serviceScope?.ServiceProvider ?? _serviceProvider;

            CliContext cliContext = InitializeCliContext(arguments, options, cancellationTokenSource);

            _logger.LogDebug("New scope created with CliContext {CliContextId}.", cliContext.Call.Identifier);

            try
            {
                // Execute middleware pipeline
                IInvokablePipelineFactory pipelineFactory = localProvider.GetRequiredService<IInvokablePipelineFactory>();
                IInvokablePipeline<CliContext> cliPipeline = pipelineFactory.GetRequiredPipeline<CliContext>();

                await cliPipeline.InvokeAsync(cliContext, cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Unhandled exception during command execution.");

                throw;
            }
            finally
            {
                _cliContextAccessor.CliContext = null;

                stopwatch.Stop();
                _logger.LogDebug("Disposed scope with CliContext {CliContextId} after {Elapsed}.", cliContext.Call.Identifier, stopwatch.Elapsed);
            }

            return cliContext.Output.ExitCode ?? ExitCode.Error;
        }

        private CliContext InitializeCliContext(IEnumerable<string> arguments,
                                                CommandExecutionOptions options,
                                                CancellationTokenSource cancellationTokenSource)
        {
            CliContext cliContext = new DefaultCliContext();

            cliContext.Features.Set<ICallInfoFeature>(new CallInfoFeature(Guid.NewGuid(), cliContext, _cliContextAccessor.CliContext));

            ICliMode instance = _cliModeAccessor.Instance ?? throw new InvalidOperationException("CliMode has not been configured for this application.");
            cliContext.Features.Set<ICliModeFeature>(new CliModeFeature(instance));
            cliContext.Features.Set<ICallLifetimeFeature>(new CallLifetimeFeature(cancellationTokenSource));

            cliContext.Features.Set<IInputFeature>(new InputFeature(arguments, options));
            cliContext.Features.Set<IOutputFeature>(new OutputFeature());

            _cliContextAccessor.CliContext = cliContext;

            return cliContext;
        }
    }
}