namespace Typin.Exceptions.Mode
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Typin mode exception.
    /// </summary>
    public abstract class ModeException : Exception
    {
        /// <summary>
        /// Initializes an instance of <see cref="ModeException"/>.
        /// </summary>
        protected ModeException()
        {

        }

        /// <summary>
        /// Initializes an instance of <see cref="ModeException"/>.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        protected ModeException(string? message) :
            base(message)
        {

        }

        /// <summary>
        /// Initializes an instance of <see cref="ModeException"/>.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        protected ModeException(string? message, Exception? innerException) :
            base(message, innerException)
        {

        }
    }
}