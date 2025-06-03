namespace Typin.Exceptions.ArgumentBinding
{
    using System;
    using Typin.Models.Schemas;

    /// <summary>
    /// Typin argument binding exception.
    /// </summary>
    public abstract class ArgumentBindingException : CliException
    {
        /// <summary>
        /// Argument schema or null.
        /// </summary>
        public IArgumentSchema? ArgumentSchema { get; }

        /// <summary>
        /// Initializes an instance of <see cref="ArgumentBindingException"/>.
        /// </summary>
        /// <param name="schema"></param>
        protected ArgumentBindingException(IArgumentSchema? schema)
        {
            ArgumentSchema = schema;
        }

        /// <summary>
        /// Initializes an instance of <see cref="ArgumentBindingException"/>.
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        protected ArgumentBindingException(IArgumentSchema? schema, string? message) : base(message)
        {
            ArgumentSchema = schema;
        }

        /// <summary>
        /// Initializes an instance of <see cref="ArgumentBindingException"/>.
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        protected ArgumentBindingException(IArgumentSchema? schema, string? message, Exception? innerException) : base(message, innerException)
        {
            ArgumentSchema = schema;
        }
    }
}