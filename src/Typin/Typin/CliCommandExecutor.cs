namespace Typin.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
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
        private IServiceScopeFactory ServiceScopeFactory { get; }

        public CliCommandExecutor(IServiceScopeFactory serviceScopeFactory)
        {
            ServiceScopeFactory = serviceScopeFactory;
        }

        /// <inheritdoc/>
        public async Task<int> ExecuteCommand(string commandLine)
        {
            string[] commandLineArguments = CommandLineSplitter.Split(commandLine).ToArray();

            return await ExecuteCommand(commandLineArguments);
        }

        /// <inheritdoc/>
        public async Task<int> ExecuteCommand(IEnumerable<string> commandLineArguments)
        {
            return await ExecuteCommand(commandLineArguments.ToList());
        }

        /// <inheritdoc/>
        [SuppressMessage("Design", "CA1031:Do not catch general exception types")]
        public async Task<int> ExecuteCommand(IReadOnlyList<string> commandLineArguments)
        {
            using (IServiceScope serviceScope = ServiceScopeFactory.CreateScope())
            {
                IServiceProvider provider = serviceScope.ServiceProvider;
                CliContext cliContext = (CliContext)provider.GetRequiredService<ICliContext>();

                CommandInput input = CommandInputResolver.Parse(commandLineArguments, cliContext.RootSchema.GetCommandNames());
                cliContext.Input = input;

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

        private static async Task RunPipelineAsync(IServiceProvider serviceProvider, CliContext cliContext)
        {
            IReadOnlyCollection<Type> middlewareTypes = cliContext.Configuration.MiddlewareTypes;

            CancellationToken cancellationToken = cliContext.Console.GetCancellationToken();
            CommandPipelineHandlerDelegate next = IMiddlewareExtensions.PipelineTermination;

            foreach (Type middlewareType in middlewareTypes)
            {
                IMiddleware instance = (IMiddleware)serviceProvider.GetRequiredService(middlewareType);
                next = instance.Next(cliContext, next, cancellationToken);
            }

            await next();
        }
    }
}
