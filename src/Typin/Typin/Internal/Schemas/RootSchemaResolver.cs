namespace Typin.Internal.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Typin.Exceptions.Resolvers.CommandResolver;
    using Typin.Exceptions.Resolvers.DirectiveResolver;
    using Typin.Schemas;

    /// <summary>
    /// Resolves an instance of <see cref="RootSchema"/>.
    /// </summary>
    internal class RootSchemaResolver
    {
        private readonly IReadOnlyCollection<Type> _commandTypes;
        private readonly IReadOnlyCollection<Type> _dynamicCommandTypes;
        private readonly IReadOnlyCollection<Type> _directiveTypes;

        public CommandSchema? DefaultCommand { get; private set; }
        public Dictionary<string, CommandSchema>? Commands { get; private set; }
        public Dictionary<string, DirectiveSchema>? Directives { get; private set; }

        /// <summary>
        /// Initializes an instance of <see cref="RootSchemaResolver"/>.
        /// </summary>
        public RootSchemaResolver(IReadOnlyCollection<Type> commandTypes,
                                  IReadOnlyCollection<Type> dynamicCommandTypes,
                                  IReadOnlyCollection<Type> directiveTypes)
        {
            _commandTypes = commandTypes;
            _dynamicCommandTypes = dynamicCommandTypes;
            _directiveTypes = directiveTypes;
        }

        /// <summary>
        /// Resolves the root schema.
        /// </summary>
        public RootSchema Resolve()
        {
            ResolveCommands(_commandTypes);
            ResolveDirectives(_directiveTypes);

            Lazy<IReadOnlyDictionary<Type, BaseCommandSchema>> _dynamicCommands = new(() =>
            {
                return _dynamicCommandTypes.ToDictionary(
                    x => x,
                    x => CommandSchemaResolver.ResolveDynamic(x));
            });

            return new RootSchema(Directives!, _dynamicCommands, Commands!, DefaultCommand);
        }

        private void ResolveCommands(IReadOnlyCollection<Type> commandTypes)
        {
            CommandSchema? defaultCommand = null;
            Dictionary<string, CommandSchema> commands = new();
            List<CommandSchema> invalidCommands = new();

            foreach (Type commandType in commandTypes)
            {
                CommandSchema command = CommandSchemaResolver.Resolve(commandType);

                if (command.IsDefault)
                {
                    if (defaultCommand is null)
                    {
                        defaultCommand = command;
                    }
                    else
                    {
                        if (!invalidCommands.Contains(defaultCommand))
                        {
                            invalidCommands.Add(defaultCommand);
                        }

                        invalidCommands.Add(command);
                    }
                }
                else if (!commands.TryAdd(command.Name!, command))
                {
                    invalidCommands.Add(command);
                }
            }

            if (commands.Count == 0 && defaultCommand is null)
            {
                defaultCommand = StubDefaultCommand.Schema;
            }

            if (invalidCommands.Count > 0)
            {
                IGrouping<string, CommandSchema> duplicateNameGroup = invalidCommands.Union(commands.Values)
                                                                                     .GroupBy(c => c.Name!, StringComparer.Ordinal)
                                                                                     .First();

                throw new CommandDuplicateByNameException(duplicateNameGroup.Key, duplicateNameGroup.ToArray());
            }

            DefaultCommand = defaultCommand;
            Commands = commands;
        }

        private void ResolveDirectives(IReadOnlyCollection<Type> directiveTypes)
        {
            Dictionary<string, DirectiveSchema> directives = new();
            List<DirectiveSchema> invalidDirectives = new();

            foreach (Type? directiveType in directiveTypes)
            {
                DirectiveSchema directive = DirectiveSchemaResolver.Resolve(directiveType);

                if (!directives.TryAdd(directive.Name, directive))
                {
                    invalidDirectives.Add(directive);
                }
            }

            if (invalidDirectives.Count > 0)
            {
                IGrouping<string, DirectiveSchema> duplicateNameGroup = invalidDirectives.Union(directives.Values)
                                                                                         .GroupBy(c => c.Name, StringComparer.Ordinal)
                                                                                         .First();

                throw new DirectiveDuplicateByNameException(duplicateNameGroup.Key, duplicateNameGroup.ToArray());
            }

            Directives = directives;
        }
    }
}