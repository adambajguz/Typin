namespace Typin.Exceptions.Resolvers
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Typin attribute exception.
    /// </summary>
    public abstract class ResolverException : CliException
    {
        /// <summary>
        /// Initializes an instance of <see cref="ResolverException"/>.
        /// </summary>
        protected ResolverException()
        {

        }

        /// <summary>
        /// Initializes an instance of <see cref="ResolverException"/>.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        protected ResolverException(string? message) : base(message)
        {

        }

        /// <summary>
        /// Initializes an instance of <see cref="ResolverException"/>.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context"><see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected ResolverException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }

        /// <summary>
        /// Initializes an instance of <see cref="ResolverException"/>.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        protected ResolverException(string? message, Exception? innerException) : base(message, innerException)
        {

        }
    }
}