namespace Typin.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Typin.Exceptions;
    using Typin.Input;
    using Typin.Internal.Extensions;
    using Typin.Internal.Input;

    internal sealed class CliCommandExecutor : ICliCommandExecutor
    {
        /// <summary>
        /// Service scope factory.
        /// </summary>
        /// <remarks>
        /// A scope is defined as a lifetime of a command execution pipeline that includes directives handling.
        /// </remarks>
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger _logger;

        public CliCommandExecutor(IServiceScopeFactory serviceScopeFactory, ILogger<CliCommandExecutor> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<int> ExecuteCommandAsync(string commandLine)
        {
            string[] commandLineArguments = CommandLineSplitter.Split(commandLine).ToArray();

            return await ExecuteCommandAsync(commandLineArguments);
        }

        /// <inheritdoc/>
        public async Task<int> ExecuteCommandAsync(IEnumerable<string> commandLineArguments)
        {
            return await ExecuteCommandAsync(commandLineArguments.ToList());
        }

        /// <inheritdoc/>
        public async Task<int> ExecuteCommandAsync(IReadOnlyList<string> commandLineArguments)
        {
            _logger.LogInformation("Executing command '{CommandLineArguments}'", commandLineArguments);

            using (IServiceScope serviceScope = _serviceScopeFactory.CreateScope())
            {
                IServiceProvider provider = serviceScope.ServiceProvider;
                ICliContext cliContext = provider.GetRequiredService<ICliContext>();

                CommandInput input = CommandInputResolver.Parse(commandLineArguments, cliContext.RootSchema.GetCommandNames());
                ((CliContext)cliContext).Input = input;

                try
                {
                    // Execute middleware pipeline
                    await RunPipelineAsync(provider, cliContext);
                }
                catch (Exception ex)
                {
                    IEnumerable<ICliExceptionHandler> exceptionHandlers = provider.GetServices<ICliExceptionHandler>();
                    foreach (ICliExceptionHandler handler in exceptionHandlers)
                    {
                        if (handler.HandleException(ex))
                            return ExitCodes.FromException(ex);
                    }

                    throw;
                }

                return cliContext.ExitCode ??= ExitCodes.Error;
            }
        }

        private static async Task RunPipelineAsync(IServiceProvider serviceProvider, ICliContext context)
        {
            IReadOnlyCollection<Type> middlewareTypes = context.Configuration.MiddlewareTypes;

            CancellationToken cancellationToken = context.Console.GetCancellationToken();
            CommandPipelineHandlerDelegate next = IMiddlewareExtensions.PipelineTermination;

            foreach (Type middlewareType in middlewareTypes.Reverse())
            {
                IMiddleware instance = (IMiddleware)serviceProvider.GetRequiredService(middlewareType);
                next = instance.Next(context, next, cancellationToken);
            }

            await next();
        }
    }
}
