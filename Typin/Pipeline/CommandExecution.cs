namespace Typin.Pipeline
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Console;
    using Typin.Exceptions;
    using Typin.Input;
    using Typin.Internal;
    using Typin.Schemas;

    internal class CommandExecution : ICliMiddleware
    {
        private readonly CliContext _cliContext;
        private readonly IServiceProvider _serviceProvider;
        private readonly HelpTextWriter _helpTextWriter;

        public CommandExecution(ICliContext cliContext, IServiceProvider serviceProvider)
        {
            _cliContext = (CliContext)cliContext;
            _serviceProvider = serviceProvider;
            _helpTextWriter = new HelpTextWriter(cliContext);
        }

        public async Task HandleAsync(ICliContext context, CommandPipelineHandlerDelegate next, CancellationToken cancellationToken)
        {
            _cliContext.ExitCode ??= await ExecuteCommand();

            await next(context, cancellationToken);
        }

        private ICommand GetCommandInstance(CommandSchema command)
        {
            return command != StubDefaultCommand.Schema ? (ICommand)_serviceProvider.GetRequiredService(command.Type) : new StubDefaultCommand();
        }

        private IDirective GetDirectiveInstance(DirectiveSchema directive)
        {
            return (IDirective)_serviceProvider.GetRequiredService(directive.Type);
        }

        private async Task<bool> ProcessDefinedDirectives(RootSchema root, CommandInput input)
        {
            bool isInteractiveMode = _cliContext.IsInteractiveMode;
            foreach (DirectiveInput directiveInput in input.Directives)
            {
                // Try to get the directive matching the input or fallback to default
                DirectiveSchema directive = root.TryFindDirective(directiveInput.Name) ?? throw TypinException.UnknownDirectiveName(directiveInput);

                if (!isInteractiveMode && directive.InteractiveModeOnly)
                    throw TypinException.InteractiveModeDirectiveNotAvailable(directiveInput.Name);

                // Get directive instance
                IDirective instance = GetDirectiveInstance(directive);

                await instance.HandleAsync(_cliContext.Console);

                if (!instance.ContinueExecution)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Executes command.
        /// </summary>
        protected async Task<int> ExecuteCommand()
        {
            IConsole _console = _cliContext.Console;
            ApplicationConfiguration _configuration = _cliContext.Configuration;
            RootSchema root = _cliContext.RootSchema;
            CommandInput input = _cliContext.Input;

            // Try to get the command matching the input or fallback to default
            CommandSchema command = root.TryFindCommand(input.CommandName) ?? StubDefaultCommand.Schema;
            _cliContext.CommandSchema = command;

            // Version option
            if (command.IsVersionOptionAvailable && input.IsVersionOptionSpecified)
            {
                _console.Output.WriteLine(_cliContext.Metadata.VersionText);

                return ExitCode.Success;
            }

            // Get command instance (also used in help text)
            ICommand instance = GetCommandInstance(command);
            _cliContext.Command = instance;

            // To avoid instantiating the command twice, we need to get default values
            // before the arguments are bound to the properties
            IReadOnlyDictionary<ArgumentSchema, object?> defaultValues = command.GetArgumentValues(instance);
            _cliContext.CommandDefaultValues = defaultValues;

            try
            {
                // Handle directives not supported in normal mode
                if (!_configuration.IsInteractiveModeAllowed && (input.HasDirective(BuiltInDirectives.Interactive)))
                    throw TypinException.InteractiveModeNotSupported();

                if (!await ProcessDefinedDirectives(_cliContext.RootSchema, _cliContext.Input))
                    return ExitCode.Success;
            }
            catch (DirectiveException ex)
            {
                _configuration.ExceptionHandler.HandleDirectiveException(_cliContext, ex);

                if (ex.ShowHelp)
                    _helpTextWriter.Write(root, command, defaultValues);

                return ExitCode.FromException(ex);
            }
            // This may throw exceptions which are useful only to the end-user
            catch (TypinException ex)
            {
                _configuration.ExceptionHandler.HandleTypinException(_cliContext, ex);

                if (ex.ShowHelp)
                    _helpTextWriter.Write(root, command, defaultValues);

                return ExitCode.FromException(ex);
            }

            // Help option
            if (command.IsHelpOptionAvailable && input.IsHelpOptionSpecified ||
                command == StubDefaultCommand.Schema && !input.Parameters.Any() && !input.Options.Any())
            {
                _helpTextWriter.Write(root, command, defaultValues); //TODO: add directives help?

                return ExitCode.Success;
            }

            // Handle directives not supported in normal mode
            if (!_configuration.IsInteractiveModeAllowed && command.InteractiveModeOnly)
            {
                throw TypinException.InteractiveOnlyCommandButThisIsNormalApplication(command);
            }
            else if (_configuration.IsInteractiveModeAllowed && command.InteractiveModeOnly &&
                     !(_cliContext.IsInteractiveMode || input.IsInteractiveDirectiveSpecified))
            {
                throw TypinException.InteractiveOnlyCommandButInteractiveModeNotStarted(command);
            }

            // Bind arguments
            try
            {
                command.Bind(instance, input, _cliContext.EnvironmentVariables);
            }
            // This may throw exceptions which are useful only to the end-user
            catch (TypinException ex)
            {
                _configuration.ExceptionHandler.HandleTypinException(_cliContext, ex);

                if (ex.ShowHelp)
                    _helpTextWriter.Write(root, command, defaultValues);

                return ExitCode.FromException(ex);
            }

            // Execute the command
            try
            {
                await _cliContext.Command.ExecuteAsync(_console);

                return _cliContext.ExitCode ??= ExitCode.Success;
            }
            // Swallow command exceptions and route them to the console
            catch (CommandException ex)
            {
                _configuration.ExceptionHandler.HandleCommandException(_cliContext, ex);

                if (ex.ShowHelp)
                    _helpTextWriter.Write(_cliContext.RootSchema, _cliContext.CommandSchema, _cliContext.CommandDefaultValues);

                return ex.ExitCode;
            }
        }
    }
}
