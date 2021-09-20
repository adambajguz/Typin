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
    using Typin.Utilities;

    /// <summary>
    /// Typin command executor.
    /// </summary>
    internal class CommandExecutor : ICommandExecutor
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ICliContextAccessor _cliContextAccessor;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="CommandExecutor"/>.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="cliContextAccessor"></param>
        /// <param name="logger"></param>
        public CommandExecutor(IServiceProvider serviceProvider, ICliContextAccessor cliContextAccessor, ILogger<CommandExecutor> logger)
        {
            _serviceProvider = serviceProvider;
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
            IEnumerable<string> commandLineArguments = CommandLineSplitter.Split(commandLine);

            return await ExecuteAsync(commandLineArguments, options, cancellationToken);
        }

        /// <summary>
        /// Executes a command.
        /// </summary>
        /// <param name="commandLineArguments"></param>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        public async Task<int> ExecuteAsync(IEnumerable<string> commandLineArguments, CommandExecutionOptions options = default, CancellationToken cancellationToken = default)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            using IServiceScope? serviceScope = options.HasFlag(CommandExecutionOptions.UseCurrentScope) ? null : _serviceProvider.CreateScope();
            IServiceProvider localProvider = serviceScope?.ServiceProvider ?? _serviceProvider;

            CliContext cliContext = new(_cliContextAccessor.CliContext, commandLineArguments, options);
            _cliContextAccessor.CliContext = cliContext;

            _logger.LogDebug("New scope created with CliContext {CliContextId}.", cliContext.Id);

            try
            {
                // Execute middleware pipeline
                IInvokablePipelineFactory pipelineFactory = localProvider.GetRequiredService<IInvokablePipelineFactory>();
                IInvokablePipeline<CliContext> cliPipeline = pipelineFactory.GetRequiredPipeline<CliContext>();

                await cliPipeline.InvokeAsync(cliContext, cancellationToken);
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
                _logger.LogDebug("Disposed scope with CliContext {CliContextId} after {Elapsed}.", cliContext.Id, stopwatch.Elapsed);
            }

            return cliContext.ExitCode ?? ExitCode.Error;
        }
    }
}