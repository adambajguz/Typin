namespace Typin.Internal.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    using Typin.Exceptions;

    /// <summary>
    /// Typin attribute exception.
    /// </summary>
    public class TemporaryException : TypinException
    {
        /// <summary>
        /// Initializes an instance of <see cref="TemporaryException"/>.
        /// </summary>
        public TemporaryException()
        {

        }

        /// <summary>
        /// Initializes an instance of <see cref="TemporaryException"/>.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public TemporaryException(string? message) : base(message)
        {

        }

        /// <summary>
        /// Initializes an instance of <see cref="TemporaryException"/>.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context"><see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        public TemporaryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }

        /// <summary>
        /// Initializes an instance of <see cref="TemporaryException"/>.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public TemporaryException(string? message, Exception? innerException) : base(message, innerException)
        {

        }
    }
}