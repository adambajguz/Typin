namespace Typin
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Console;
    using Typin.Exceptions;
    using Typin.Input;
    using Typin.Internal.Extensions;
    using Typin.Schemas;

    /// <summary>
    /// Command line application facade.
    /// </summary>
    public partial class CliApplication
    {
        /// <summary>
        /// Services provider.
        /// </summary>
        protected IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Service scope factory.
        /// <remarks>
        /// A scope is defined as a lifetime of a command execution pipeline that includes directives handling.
        /// </remarks>
        /// </summary>
        protected IServiceScopeFactory ServiceScopeFactory { get; }

        /// <summary>
        /// Cli Context instance.
        /// </summary>
        protected CliContext CliContext { get; }

        private readonly ApplicationMetadata _metadata;
        private readonly ApplicationConfiguration _configuration;
        private readonly IConsole _console;

        private readonly LinkedList<Type> _middlewareTypes;

        /// <summary>
        /// Initializes an instance of <see cref="CliApplication"/>.
        /// </summary>
        public CliApplication(LinkedList<Type> middlewareTypes,
                              IServiceProvider serviceProvider,
                              CliContext cliContext)
        {
            ServiceProvider = serviceProvider;
            ServiceScopeFactory = serviceProvider.GetService<IServiceScopeFactory>();

            CliContext = cliContext;

            _metadata = cliContext.Metadata;
            _configuration = cliContext.Configuration;
            _console = cliContext.Console;

            _middlewareTypes = middlewareTypes;
        }

        private (LinkedList<IMiddleware> middlewares, CommandPipelineHandlerDelegate runPipelineAsync) GetMiddlewarePipeline(IServiceScope serviceScope, LinkedList<Type> middlewareTypes)
        {
            CancellationToken cancellationToken = _console.GetCancellationToken();
            CommandPipelineHandlerDelegate next = ICliMiddlewareExtensions.PipelineTermination;

            LinkedList<IMiddleware> middlewareComponenets = new LinkedList<IMiddleware>();
            foreach (Type middlewareType in middlewareTypes)
            {
                IMiddleware instance = (IMiddleware)serviceScope.ServiceProvider.GetRequiredService(middlewareType);
                next = instance.Next(CliContext, next, cancellationToken);

                middlewareComponenets.AddFirst(instance);
            }

            return (middlewareComponenets, next);
        }

        /// <summary>
        /// Prints the startup message if availble.
        /// </summary>
        protected void PrintStartupMessage()
        {
            if (_metadata.StartupMessage is null)
                return;

            _console.WithForegroundColor(ConsoleColor.Blue, () => _console.Output.WriteLine(_metadata.StartupMessage));
        }

        #region Run
        /// <summary>
        /// Runs the application and returns the exit code.
        /// Command line arguments and environment variables are retrieved automatically.
        /// </summary>
        /// <remarks>
        /// If a <see cref="CommandException"/> is thrown during command execution, it will be handled and routed to the console.
        /// Additionally, if the debugger is not attached (i.e. the app is running in production), all other exceptions thrown within
        /// this method will be handled and routed to the console as well.
        /// </remarks>
        public async ValueTask<int> RunAsync()
        {
            string[] commandLineArguments = Environment.GetCommandLineArgs()
                                                       .Skip(1)
                                                       .ToArray();

            return await RunAsync(commandLineArguments);
        }

        /// <summary>
        /// Runs the application with specified command line arguments and returns the exit code.
        /// Environment variables are retrieved automatically.
        /// </summary>
        /// <remarks>
        /// If a <see cref="CommandException"/> is thrown during command execution, it will be handled and routed to the console.
        /// Additionally, if the debugger is not attached (i.e. the app is running in production), all other exceptions thrown within
        /// this method will be handled and routed to the console as well.
        /// </remarks>
        public async ValueTask<int> RunAsync(IReadOnlyList<string> commandLineArguments)
        {
            // Environment variable names are case-insensitive on Windows but are case-sensitive on Linux and macOS
            Dictionary<string, string> environmentVariables = Environment.GetEnvironmentVariables()
                                                                         .Cast<DictionaryEntry>()
                                                                         .ToDictionary(x => (string)x.Key,
                                                                                       x => (string)x.Value,
                                                                                       StringComparer.Ordinal);

            return await RunAsync(commandLineArguments, environmentVariables);
        }

        /// <summary>
        /// Runs the application with specified command line arguments and environment variables, and returns the exit code.
        /// </summary>
        /// <remarks>
        /// If a <see cref="CommandException"/> or <see cref="TypinException"/> is thrown during command execution, it will be handled and routed to the console.
        /// Additionally, if the debugger is not attached (i.e. the app is running in production), all other exceptions thrown within
        /// this method will be handled and routed to the console as well.
        /// </remarks>
        public async ValueTask<int> RunAsync(IReadOnlyList<string> commandLineArguments,
                                             IReadOnlyDictionary<string, string> environmentVariables)
        {
            try
            {
                _console.ResetColor();
                _console.ForegroundColor = ConsoleColor.Gray;

                CliContext.EnvironmentVariables = environmentVariables;

                PrintStartupMessage();

                RootSchema root = RootSchema.Resolve(_configuration.CommandTypes, _configuration.DirectiveTypes);
                CliContext.RootSchema = root;

                int exitCode = await PreExecuteCommand(commandLineArguments, root);

                return exitCode;
            }
            // To prevent the app from showing the annoying Windows troubleshooting dialog,
            // we handle all exceptions and route them to the console nicely.
            // However, we don't want to swallow unhandled exceptions when the debugger is attached,
            // because we still want the IDE to show them to the developer.
            catch (Exception ex) when (!Debugger.IsAttached)
            {
                _configuration.ExceptionHandler.HandleException(CliContext, ex);

                return ExitCodes.FromException(ex);
            }
        }

        /// <summary>
        /// Runs before command execution.
        /// </summary>
        protected virtual async Task<int> PreExecuteCommand(IReadOnlyList<string> commandLineArguments,
                                                            RootSchema root)
        {
            CommandInput input = CommandInput.Parse(commandLineArguments, root.GetCommandNames());
            CliContext.Input = input;

            return await ExecuteCommand();
        }

        /// <summary>
        /// Executes command.
        /// </summary>
        protected async Task<int> ExecuteCommand()
        {
            // Create service scope
            using (IServiceScope serviceScope = ServiceScopeFactory.CreateScope())
            {
                (LinkedList<IMiddleware> middlewares, CommandPipelineHandlerDelegate runPipelineAsync) = GetMiddlewarePipeline(serviceScope, _middlewareTypes);
                await runPipelineAsync(CliContext, _console.GetCancellationToken());
            }

            //TODO: CliContext.BeginExecutionContext() would be more elegant
            CliContext.Input = default!;
            CliContext.Command = default!;
            CliContext.CommandDefaultValues = default!;
            CliContext.CommandSchema = default!;

            int exitCode = CliContext.ExitCode ??= ExitCodes.Error;
            CliContext.ExitCode = null;

            return exitCode;
        }
        #endregion
    }
}
