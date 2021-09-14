namespace Typin.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using PackSite.Library.Pipelining;
    using Typin.Input;
    using Typin.Internal.Input;
    using Typin.Utilities;

    internal sealed class CliCommandExecutor : ICliCommandExecutor
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IRootSchemaAccessor _rootSchemaAccessor;
        private readonly ICliContextAccessor _cliContextAccessor;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="CliCommandExecutor"/>.
        /// </summary>
        public CliCommandExecutor(IServiceScopeFactory serviceScopeFactory,
                                  IRootSchemaAccessor rootSchemaAccessor,
                                  ICliContextAccessor cliContextAccessor,
                                  ILogger<CliCommandExecutor> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _rootSchemaAccessor = rootSchemaAccessor;
            _cliContextAccessor = cliContextAccessor;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<int> ExecuteCommandAsync(string commandLine, bool startsWithExecutable = false, CancellationToken cancellationToken = default)
        {
            IEnumerable<string> commandLineArguments = CommandLineSplitter.Split(commandLine);

            return await ExecuteCommandAsync(commandLineArguments, startsWithExecutable, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<int> ExecuteCommandAsync(IEnumerable<string> commandLineArguments, bool containsExecutable = false, CancellationToken cancellationToken = default)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            commandLineArguments = commandLineArguments.Skip(containsExecutable ? 1 : 0);

            using (IServiceScope serviceScope = _serviceScopeFactory.CreateScope())
            {
                IServiceProvider provider = serviceScope.ServiceProvider;

                CliContext cliContext = new();
                _cliContextAccessor.CliContext = cliContext;

                _logger.LogDebug("New scope created with CliContext {CliContextId}.", cliContext.Id);

                CommandInput input = InputResolver.Parse(commandLineArguments, _rootSchemaAccessor.RootSchema.GetCommandNames());
                cliContext.Input = input;

                try
                {
                    // Execute middleware pipeline
                    IInvokablePipelineFactory pipelineFactory = provider.GetRequiredService<IInvokablePipelineFactory>();
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
}
