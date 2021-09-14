﻿namespace Typin.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Domain exception thrown within Typin.
    /// </summary>
    public abstract class TypinException : Exception
    {
        /// <summary>
        /// Initializes an instance of <see cref="TypinException"/>.
        /// </summary>
        protected TypinException()
        {

        }

        /// <summary>
        /// Initializes an instance of <see cref="TypinException"/>.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        protected TypinException(string? message) : base(message)
        {

        }

        /// <summary>
        /// Initializes an instance of <see cref="TypinException"/>.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context"><see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected TypinException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }

        /// <summary>
        /// Initializes an instance of <see cref="TypinException"/>.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        protected TypinException(string? message, Exception? innerException) : base(message, innerException)
        {

        }
    }
}