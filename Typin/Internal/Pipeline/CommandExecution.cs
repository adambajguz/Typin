﻿namespace Typin.Internal.Pipeline
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Typin;
    using Typin.Exceptions;
    using Typin.Input;
    using Typin.Internal;
    using Typin.OptionFallback;
    using Typin.Schemas;

    internal sealed class CommandExecution : IMiddleware
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IOptionFallbackProvider _optionFallbackProvider;

        public CommandExecution(IServiceProvider serviceProvider, IOptionFallbackProvider optionFallbackProvider)
        {
            _serviceProvider = serviceProvider;
            _optionFallbackProvider = optionFallbackProvider;
        }

        public async Task HandleAsync(ICliContext context, CommandPipelineHandlerDelegate next, CancellationToken cancellationToken)
        {
            context.ExitCode ??= await ExecuteCommand((CliContext)context);

            await next();
        }

        private ICommand GetCommandInstance(CommandSchema command)
        {
            return command != StubDefaultCommand.Schema ? (ICommand)_serviceProvider.GetRequiredService(command.Type) : new StubDefaultCommand();
        }

        private IDirective GetDirectiveInstance(DirectiveSchema directive)
        {
            return (IDirective)_serviceProvider.GetRequiredService(directive.Type);
        }

        private async Task<bool> ProcessDefinedDirectives(ICliContext context)
        {
            bool isInteractiveMode = context.IsInteractiveMode;
            IReadOnlyList<DirectiveInput> directives = context.Input.Directives;
            foreach (DirectiveInput directiveInput in directives)
            {
                // Try to get the directive matching the input or fallback to default
                DirectiveSchema directive = context.RootSchema.TryFindDirective(directiveInput.Name) ?? throw TypinException.UnknownDirectiveName(directiveInput);

                if (!isInteractiveMode && directive.InteractiveModeOnly)
                    throw TypinException.InteractiveModeDirectiveNotAvailable(directiveInput.Name);

                // Get directive instance
                IDirective instance = GetDirectiveInstance(directive);

                await instance.HandleAsync(context.Console);

                if (!instance.ContinueExecution)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Executes command.
        /// </summary>
        private async Task<int> ExecuteCommand(CliContext context)
        {
            ApplicationConfiguration _configuration = context.Configuration;
            RootSchema root = context.RootSchema;
            CommandInput input = context.Input;

            // Try to get the command matching the input or fallback to default
            bool hasDefaultDirective = input.HasDirective(BuiltInDirectives.Default);
            CommandSchema command = root.TryFindCommand(input.CommandName, hasDefaultDirective) ?? StubDefaultCommand.Schema;

            // Forbid to execute real default command in interactive mode without [!] directive.
            if (!(command.IsHelpOptionAvailable && input.IsHelpOptionSpecified) &&
                context.IsInteractiveMode && command.IsDefault && !hasDefaultDirective)
            {
                command = StubDefaultCommand.Schema;
            }

            // Update CommandSchema
            context.CommandSchema = command;

            // Version option
            if (command.IsVersionOptionAvailable && input.IsVersionOptionSpecified)
            {
                context.Console.Output.WriteLine(context.Metadata.VersionText);

                return ExitCodes.Success;
            }

            // Get command instance (also used in help text)
            ICommand instance = GetCommandInstance(command);
            context.Command = instance;

            // To avoid instantiating the command twice, we need to get default values
            // before the arguments are bound to the properties
            IReadOnlyDictionary<ArgumentSchema, object?> defaultValues = command.GetArgumentValues(instance);
            context.CommandDefaultValues = defaultValues;

            // Handle interactive directive not supported in normal mode
            if (!_configuration.IsInteractiveModeAllowed && input.IsInteractiveDirectiveSpecified)
                throw TypinException.InteractiveModeNotSupported();

            // Help option
            if ((command.IsHelpOptionAvailable && input.IsHelpOptionSpecified) ||
                (command == StubDefaultCommand.Schema && !input.Parameters.Any() && !input.Options.Any()))
            {
                HelpTextWriter helpTextWriter = new HelpTextWriter(context);
                helpTextWriter.Write(root, command, defaultValues); //TODO: add directives help?

                return ExitCodes.Success;
            }

            // Handle directives
            if (!await ProcessDefinedDirectives(context))
                return ExitCodes.Success;

            // Handle commands not supported in normal mode
            if (!_configuration.IsInteractiveModeAllowed && command.InteractiveModeOnly)
            {
                throw TypinException.InteractiveOnlyCommandButThisIsNormalApplication(command);
            }
            else if (_configuration.IsInteractiveModeAllowed && command.InteractiveModeOnly &&
                     !(context.IsInteractiveMode || input.IsInteractiveDirectiveSpecified))
            {
                throw TypinException.InteractiveOnlyCommandButInteractiveModeNotStarted(command);
            }

            // Bind arguments
            command.Bind(instance, input, _optionFallbackProvider);

            // Execute command
            await instance.ExecuteAsync(context.Console);

            return context.ExitCode ??= ExitCodes.Success;
        }
    }
}