﻿namespace Typin.Internal.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Typin.Internal.Exceptions;
    using Typin.Schemas;

    /// <summary>
    /// Resolves an instance of <see cref="RootSchema"/>.
    /// </summary>
    internal class RootSchemaResolver
    {
        private readonly IReadOnlyList<Type> _commandTypes;
        private readonly IReadOnlyList<Type> _directiveTypes;
        private readonly IReadOnlyList<Type> _modeTypes;

        public CommandSchema? DefaultCommand { get; private set; }
        public Dictionary<string, CommandSchema>? Commands { get; private set; }
        public Dictionary<string, DirectiveSchema>? Directives { get; private set; }

        /// <summary>
        /// Initializes an instance of <see cref="RootSchemaResolver"/>.
        /// </summary>
        public RootSchemaResolver(IReadOnlyList<Type> commandTypes, IReadOnlyList<Type> directiveTypes, IReadOnlyList<Type> modeTypes)
        {
            _commandTypes = commandTypes;
            _directiveTypes = directiveTypes;
            _modeTypes = modeTypes;
        }

        /// <summary>
        /// Resolves the root schema.
        /// </summary>
        public RootSchema Resolve()
        {
            ResolveCommands(_commandTypes);
            ResolveDirectives(_directiveTypes);

            return new RootSchema(Directives!, Commands!, DefaultCommand);
        }

        private void ResolveCommands(IReadOnlyList<Type> commandTypes)
        {
            CommandSchema? defaultCommand = null;
            var commands = new Dictionary<string, CommandSchema>();
            var invalidCommands = new List<CommandSchema>();

            foreach (Type commandType in commandTypes)
            {
                CommandSchema command = CommandSchemaResolver.Resolve(commandType, _modeTypes);

                if (command.IsDefault)
                {
                    defaultCommand = defaultCommand is null ? command : throw CommandResolverExceptions.TooManyDefaultCommands();
                }
                else if (!commands.TryAdd(command.Name!, command))
                {
                    invalidCommands.Add(command);
                }
            }

            if (commands.Count == 0 && defaultCommand is null)
                throw CommandResolverExceptions.NoCommandsDefined();

            if (invalidCommands.Count > 0)
            {
                IGrouping<string, CommandSchema> duplicateNameGroup = invalidCommands.Union(commands.Values)
                                                                                     .GroupBy(c => c.Name!, StringComparer.Ordinal)
                                                                                     .First();

                throw CommandResolverExceptions.CommandsWithSameName(duplicateNameGroup.Key, duplicateNameGroup.ToArray());
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
                DirectiveSchema directive = DirectiveSchemaResolver.Resolve(directiveType, _modeTypes);

                if (!directives.TryAdd(directive.Name, directive))
                    invalidDirectives.Add(directive);
            }

            if (invalidDirectives.Count > 0)
            {
                IGrouping<string, DirectiveSchema> duplicateNameGroup = invalidDirectives.Union(directives.Values)
                                                                                         .GroupBy(c => c.Name, StringComparer.Ordinal)
                                                                                         .First();

                throw DirectiveResolverExceptions.DirectiveWithSameName(duplicateNameGroup.Key, duplicateNameGroup.ToArray());
            }

            Directives = directives;
        }
    }
}