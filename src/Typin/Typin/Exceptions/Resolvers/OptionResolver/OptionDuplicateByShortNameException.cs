namespace Typin.Exceptions.Resolvers.OptionResolver
{
    using System;
    using System.Collections.Generic;
    using Typin.Internal.Extensions;
    using Typin.Schemas;

    /// <summary>
    /// Option duplicate by short name exception.
    /// </summary>
    public sealed class OptionDuplicateByShortNameException : OptionResolverException
    {
        /// <summary>
        /// Duplicated options short name.
        /// </summary>
        public char ShortName { get; set; }

        /// <summary>
        /// Initializes an instance of <see cref="OptionDuplicateByShortNameException"/>.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="shortName"></param>
        /// <param name="invalidOptions"></param>
        public OptionDuplicateByShortNameException(BaseCommandSchema command, char shortName, IReadOnlyList<OptionSchema> invalidOptions) :
            base(command,
                 invalidOptions,
                 BuildMessage(command, shortName, invalidOptions))
        {
            ShortName = shortName;
        }

        private static string BuildMessage(BaseCommandSchema command, char shortName, IReadOnlyList<OptionSchema> invalidOptions)
        {
            return $"Command '{command.Type.FullName}' is invalid because it contains {invalidOptions.Count} options with the same short name ('{shortName}'):{Environment.NewLine}" +
                   $"{invalidOptions.JoinToString(Environment.NewLine)}{Environment.NewLine}" +
                   Environment.NewLine +
                   "Options must have unique short names (short names are case-sensitive).";
        }
    }
}