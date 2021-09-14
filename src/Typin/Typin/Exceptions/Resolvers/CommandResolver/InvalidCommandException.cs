namespace Typin.Exceptions.Resolvers.CommandResolver
{
    using System;
    using Typin;
    using Typin.Attributes;

    /// <summary>
    /// Invalid command exception.
    /// </summary>
    public sealed class InvalidCommandException : CommandResolverException
    {
        /// <summary>
        /// Command type.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Initializes an instance of <see cref="InvalidCommandException"/>.
        /// </summary>
        /// <param name="type"></param>
        public InvalidCommandException(Type type) :
            base(BuildMessage(type))
        {
            Type = type;
        }

        private static string BuildMessage(Type type)
        {
            return $"Command '{type.FullName}' is not a valid command type.{Environment.NewLine}" +
                   Environment.NewLine +
                   $"In order to be a valid command type, it must:{Environment.NewLine}" +
                   $"  - Not be an abstract class{Environment.NewLine}" +
                   $"  - Implement {typeof(ICommand).FullName}{Environment.NewLine}" +
                   $"  - Be annotated with {typeof(CommandAttribute).FullName}.";
        }
    }
}