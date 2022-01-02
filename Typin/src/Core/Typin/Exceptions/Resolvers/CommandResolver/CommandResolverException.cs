namespace Typin.Exceptions.Resolvers.CommandResolver
{
    using System;
    using System.Runtime.Serialization;
    using Typin.Exceptions.Resolvers;

    /// <summary>
    /// Typin command resolver exception.
    /// </summary>
    public abstract class CommandResolverException : ResolverException
    {
        /// <summary>
        /// Initializes an instance of <see cref="CommandResolverException"/>.
        /// </summary>
        protected CommandResolverException()
        {

        }

        /// <summary>
        /// Initializes an instance of <see cref="CommandResolverException"/>.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        protected CommandResolverException(string? message) : base(message)
        {

        }

        /// <summary>
        /// Initializes an instance of <see cref="CommandResolverException"/>.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context"><see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected CommandResolverException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }

        /// <summary>
        /// Initializes an instance of <see cref="CommandResolverException"/>.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        protected CommandResolverException(string? message, Exception? innerException) : base(message, innerException)
        {

        }
    }
}