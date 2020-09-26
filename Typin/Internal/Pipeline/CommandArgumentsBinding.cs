namespace Typin.Internal.Pipeline
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Exceptions;
    using Typin.Input;
    using Typin.Internal;
    using Typin.OptionFallback;
    using Typin.Schemas;

    internal sealed class CommandArgumentsBinding : IMiddleware
    {
        private readonly IOptionFallbackProvider _optionFallbackProvider;

        public CommandArgumentsBinding(IOptionFallbackProvider optionFallbackProvider)
        {
            _optionFallbackProvider = optionFallbackProvider;
        }

        public async Task HandleAsync(ICliContext context, CommandPipelineHandlerDelegate next, CancellationToken cancellationToken)
        {
            context.ExitCode ??= ExecuteCommand(context);

            await next();
        }

        /// <summary>
        /// Executes command.
        /// </summary>
        private int? ExecuteCommand(ICliContext context)
        {
            // Get command input, schema and instance from CliContext
            CommandInput input = context.Input;
            ICommand instance = context.Command;
            CommandSchema command = context.CommandSchema;

            // Bind arguments
            try
            {
                command.Bind(instance, input, _optionFallbackProvider);
            }
            // This may throw exceptions which are useful only to the end-user
            catch (TypinException ex)
            {
                ApplicationConfiguration _configuration = context.Configuration;
                _configuration.ExceptionHandler.HandleTypinException(context, ex);

                if (ex.ShowHelp)
                {
                    // Get command default values from CliContext
                    IReadOnlyDictionary<ArgumentSchema, object?> defaultValues = context.CommandDefaultValues;

                    // Print help
                    HelpTextWriter helpTextWriter = new HelpTextWriter(context);
                    helpTextWriter.Write(context.RootSchema, command, defaultValues);
                }

                return ExitCodes.FromException(ex);
            }

            return null;
        }
    }
}
