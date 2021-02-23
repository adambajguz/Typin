namespace Typin.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Typin.Exceptions;
    using Typin.Input;
    using Typin.Internal.Extensions;
    using Typin.Internal.Input;
    using Typin.Utilities;

    internal sealed class CliCommandExecutor : ICliCommandExecutor
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IRootSchemaAccessor _rootSchemaAccessor;
        private readonly ILogger _logger;

        public CliCommandExecutor(IServiceScopeFactory serviceScopeFactory,
                                  IRootSchemaAccessor rootSchemaAccessor,
                                  ILogger<CliCommandExecutor> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _rootSchemaAccessor = rootSchemaAccessor;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<int> ExecuteCommandAsync(string commandLine)
        {
            IEnumerable<string> commandLineArguments = CommandLineSplitter.Split(commandLine);

            return await ExecuteCommandAsync(commandLineArguments);
        }

        /// <inheritdoc/>
        public async Task<int> ExecuteCommandAsync(IEnumerable<string> commandLineArguments)
        {
            _logger.LogInformation("Executing command '{CommandLineArguments}'.", commandLineArguments);

            using (IServiceScope serviceScope = _serviceScopeFactory.CreateScope())
            {
                IServiceProvider provider = serviceScope.ServiceProvider;
                ICliContext cliContext = provider.GetRequiredService<ICliContext>();
                Guid cliContextId = cliContext.Id;

                _logger.LogDebug("New scope created with CliContext {CliContextId}.", cliContextId);

                CommandInput input = CommandInputResolver.Parse(commandLineArguments, _rootSchemaAccessor.RootSchema.GetCommandNames());
                ((CliContext)cliContext).Input = input;

                try
                {
                    // Execute middleware pipeline
                    await RunPipelineAsync(provider, cliContext);
                }
                catch (Exception ex)
                {
                    _logger.LogDebug("Exception occured. Trying to find exception handler.");

                    IEnumerable<ICliExceptionHandler> exceptionHandlers = provider.GetServices<ICliExceptionHandler>();
                    foreach (ICliExceptionHandler handler in exceptionHandlers)
                    {
                        if (handler.HandleException(ex))
                        {
                            _logger.LogDebug(ex, "Exception handled by {ExceptionHandlerType}.", handler.GetType().FullName);

                            return ExitCodes.FromException(ex);
                        }
                    }

                    _logger.LogCritical(ex, "Unhandled exception during command execution.");

                    throw;
                }
                finally
                {
                    _logger.LogDebug("Disposed scope with CliContext {CliContextId}.", cliContextId);
                }

                return cliContext.ExitCode ?? ExitCodes.Error;
            }
        }

        private async Task RunPipelineAsync(IServiceProvider serviceProvider, ICliContext context)
        {
            IReadOnlyCollection<Type> middlewareTypes = context.Configuration.MiddlewareTypes;

            CancellationToken cancellationToken = context.Console.GetCancellationToken();
            CommandPipelineHandlerDelegate next = IMiddlewareExtensions.PipelineTermination;

            foreach (Type middlewareType in middlewareTypes.Reverse())
            {
                IMiddleware instance = (IMiddleware)serviceProvider.GetRequiredService(middlewareType);
                next = instance.Next(context, next, middlewareType, _logger, cancellationToken);
            }

            await next();
        }
    }
}
