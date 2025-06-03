namespace Typin.Commands.Resolvers
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
    using Typin.Commands;
    using Typin.Features;
    using Typin.Modes;
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
        /// <param name="inputOptions"></param>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        public async Task<int> ExecuteAsync(string commandLine,
                                            InputOptions inputOptions = default,
                                            ModeBehavior options = default,
                                            CancellationToken cancellationToken = default)
        {
            IEnumerable<string> commandLineArguments = CommandLine.Split(commandLine);

            return await ExecuteAsync(commandLineArguments, inputOptions, options, cancellationToken);
        }

        /// <summary>
        /// Executes a command.
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="inputOptions"></param>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        public async Task<int> ExecuteAsync(IEnumerable<string> arguments,
                                            InputOptions inputOptions = default,
                                            ModeBehavior options = default,
                                            CancellationToken cancellationToken = default)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            using CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            await using AsyncServiceScope? serviceScope = options.HasFlag(ModeBehavior.UseCurrentScope)
                ? null
                : _serviceProvider.CreateAsyncScope();

            IServiceProvider localProvider = serviceScope?.ServiceProvider ?? _serviceProvider;

            CliContext cliContext = InitializeCliContext(localProvider, arguments, inputOptions, cancellationTokenSource);

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

        private CliContext InitializeCliContext(IServiceProvider localProvider,
                                                IEnumerable<string> arguments,
                                                InputOptions inputOptions,
                                                CancellationTokenSource cancellationTokenSource)
        {
            ICliMode instance = _cliModeAccessor.Instance ?? throw new InvalidOperationException("CliMode has not been configured for this application.");

            CliContext cliContext = new DefaultCliContext(
                    new CallServicesFeature(localProvider),
                    _cliContextAccessor.CliContext,
                    new CliModeFeature(instance),
                    new CallLifetimeFeature(cancellationTokenSource),
                    new InputFeature(arguments, inputOptions),
                    new OutputFeature()
                );

            _cliContextAccessor.CliContext = cliContext;

            return cliContext;
        }
    }
}