namespace Typin.Exceptions.Resolvers.CommandResolver
{
    using System;
    using System.Collections.Generic;
    using Typin.Internal.Extensions;
    using Typin.Schemas;

    /// <summary>
    /// Command duplicate by name exception.
    /// </summary>
    public sealed class CommandDuplicateByNameException : CommandResolverException
    {
        /// <summary>
        /// Duplicated command name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Duplicated commands name.
        /// </summary>
        public IReadOnlyList<CommandSchema> InvalidCommands { get; set; }

        /// <summary>
        /// Initializes an instance of <see cref="CommandDuplicateByNameException"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="invalidCommands"></param>
        public CommandDuplicateByNameException(string name, IReadOnlyList<CommandSchema> invalidCommands) :
            base(BuildMessage(name, invalidCommands))
        {
            Name = name;
            InvalidCommands = invalidCommands;
        }

        private static string BuildMessage(string name, IReadOnlyList<CommandSchema> invalidCommands)
        {
            return $"Application configuration is invalid because it contains {invalidCommands.Count} commands with the same name ('{name}'):{Environment.NewLine}" +
                   $"{invalidCommands.JoinToString(Environment.NewLine)}{Environment.NewLine}" +
                   Environment.NewLine +
                   "Commands must have unique names (names are case-sensitive).";
        }
    }
}