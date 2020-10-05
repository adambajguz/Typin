namespace Typin.Schemas.Resolvers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Typin.Internal.Exceptions;

    /// <summary>
    /// Resolves an instance of <see cref="RootSchema"/>.
    /// </summary>
    internal class RootSchemaResolver : IResolver<RootSchema>
    {
        private ApplicationConfiguration Configuration { get; }
        private IReadOnlyList<Type> CommandTypes { get; }
        private IReadOnlyList<Type> DirectiveTypes { get; }

        public bool IsResolved { get; private set; }

        public CommandSchema? DefaultCommand { get; private set; }
        public Dictionary<string, CommandSchema>? Commands { get; private set; }
        public Dictionary<string, DirectiveSchema>? Directives { get; private set; }

        /// <summary>
        /// Initializes an instance of <see cref="RootSchemaResolver"/>.
        /// </summary>
        public RootSchemaResolver(ApplicationConfiguration configuration)
        {
            Configuration = configuration;
            CommandTypes = configuration.CommandTypes;
            DirectiveTypes = configuration.DirectiveTypes;
        }

        /// <summary>
        /// Resolves the root schema.
        /// </summary>
        public RootSchema Resolve()
        {
            ResolveCommands(CommandTypes);
            ResolveDirectives(DirectiveTypes);

            IsResolved = true;

            return new RootSchema(Directives!, Commands!, DefaultCommand);
        }

        private void ResolveCommands(IReadOnlyList<Type> commandTypes)
        {
            CommandSchema? defaultCommand = null;
            var commands = new Dictionary<string, CommandSchema>();
            var invalidCommands = new List<CommandSchema>();

            foreach (Type commandType in commandTypes)
            {
                CommandSchema command = CommandSchema.TryResolve(commandType) ?? throw InternalTypinExceptions.InvalidCommandType(commandType);

                if (string.IsNullOrWhiteSpace(command.Name))
                {
                    defaultCommand = defaultCommand is null ? command : throw InternalTypinExceptions.TooManyDefaultCommands();
                }
                else if (!commands.TryAdd(command.Name, command))
                {
                    invalidCommands.Add(command);
                }
            }

            if (commands.Count == 0 && defaultCommand is null)
                throw InternalTypinExceptions.NoCommandsDefined();

            if (invalidCommands.Count > 0)
            {
                var duplicateNameGroup = invalidCommands.Union(commands.Values)
                                                        .GroupBy(c => c.Name!, StringComparer.OrdinalIgnoreCase)
                                                        .FirstOrDefault();

                throw InternalTypinExceptions.CommandsWithSameName(duplicateNameGroup.Key, duplicateNameGroup.ToArray());
            }

            DefaultCommand = defaultCommand;
            Commands = commands;
        }

        private void ResolveDirectives(IReadOnlyList<Type> directiveTypes)
        {
            var directives = new Dictionary<string, DirectiveSchema>();
            var invalidDirectives = new List<DirectiveSchema>();

            foreach (Type? directiveType in directiveTypes)
            {
                DirectiveSchema directive = DirectiveSchema.TryResolve(directiveType) ?? throw InternalTypinExceptions.InvalidDirectiveType(directiveType);

                if (!directives.TryAdd(directive.Name, directive))
                    invalidDirectives.Add(directive);
            }

            if (invalidDirectives.Count > 0)
            {
                var duplicateNameGroup = invalidDirectives.Union(directives.Values)
                                                          .GroupBy(c => c.Name, StringComparer.OrdinalIgnoreCase)
                                                          .FirstOrDefault();

                throw InternalTypinExceptions.DirectiveWithSameName(duplicateNameGroup.Key, duplicateNameGroup.ToArray());
            }

            Directives = directives;
        }
    }
}