namespace Typin.Exceptions.Resolvers.CommandResolver
{
    using System;
    using Typin;

    /// <summary>
    /// Invalid dynamic command exception.
    /// </summary>
    public sealed class InvalidDynamicCommandException : CommandResolverException
    {
        /// <summary>
        /// Command type.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Initializes an instance of <see cref="InvalidCommandException"/>.
        /// </summary>
        /// <param name="type"></param>
        public InvalidDynamicCommandException(Type type) :
            base(BuildMessage(type))
        {
            Type = type;
        }

        private static string BuildMessage(Type type)
        {
            return $"Command '{type.FullName}' is not a valid command type.{Environment.NewLine}" +
                   Environment.NewLine +
                   $"In order to be a valid dynamic command type, it must:{Environment.NewLine}" +
                   $"  - Not be an abstract class{Environment.NewLine}" +
                   $"  - Implement {typeof(ICommandTemplate).FullName}{Environment.NewLine}.";
        }
    }
}