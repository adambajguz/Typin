namespace Typin.Internal.Pipeline
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Console;
    using Typin.Exceptions;
    using Typin.Internal;

    internal sealed class CommandExecution : IMiddleware
    {
        public CommandExecution()
        {

        }

        public async Task HandleAsync(ICliContext context, CommandPipelineHandlerDelegate next, CancellationToken cancellationToken)
        {
            context.ExitCode ??= await ExecuteCommand(context);

            await next();
        }

        /// <summary>
        /// Executes command.
        /// </summary>
        private async Task<int> ExecuteCommand(ICliContext context)
        {
            // Get console and command instance
            IConsole _console = context.Console;
            ICommand instance = context.Command;

            // Execute the command
            try
            {
                await instance.ExecuteAsync(_console);

                return context.ExitCode ??= ExitCodes.Success;
            }
            // Swallow command exceptions and route them to the console
            catch (CommandException ex)
            {
                ApplicationConfiguration _configuration = context.Configuration;
                _configuration.ExceptionHandler.HandleCommandException(context, ex);

                if (ex.ShowHelp)
                {
                    HelpTextWriter _helpTextWriter = new HelpTextWriter(context);
                    _helpTextWriter.Write(context.RootSchema, context.CommandSchema, context.CommandDefaultValues);
                }
                return ex.ExitCode;
            }
        }
    }
}
