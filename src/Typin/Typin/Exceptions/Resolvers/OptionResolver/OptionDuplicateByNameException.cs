namespace Typin.Exceptions.Resolvers.OptionResolver
{
    using System;
    using System.Collections.Generic;
    using Typin.Models.Binding;
    using Typin.Models.Schemas;
    using Typin.Utilities.Extensions;

    /// <summary>
    /// Option duplicate by name exception.
    /// </summary>
    public sealed class OptionDuplicateByNameException : OptionResolverException
    {
        /// <summary>
        /// Duplicated options name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Initializes an instance of <see cref="OptionDuplicateByNameException"/>.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="invalidOptions"></param>
        public OptionDuplicateByNameException(IModelSchema command, string name, IReadOnlyList<IOptionSchema> invalidOptions) :
            base(command,
                 invalidOptions,
                 BuildMessage(command, name, invalidOptions))
        {
            Name = name;
        }

        private static string BuildMessage(IModelSchema command, string name, IReadOnlyList<IOptionSchema> invalidOptions)
        {
            return $"Command '{command.Type.FullName}' is invalid because it contains {invalidOptions.Count} options with the same name ('{name}'):{Environment.NewLine}" +
                   $"{invalidOptions.JoinToString(Environment.NewLine)}{Environment.NewLine}" +
                   Environment.NewLine +
                   "Options must have unique names (names are case-sensitive).";
        }
    }
}