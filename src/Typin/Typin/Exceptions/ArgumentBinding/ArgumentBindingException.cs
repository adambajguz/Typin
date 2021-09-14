﻿namespace Typin.Exceptions.ArgumentBinding
{
    using System;
    using System.Runtime.Serialization;
    using Typin.Exceptions;
    using Typin.Input;
    using Typin.Schemas;

    /// <summary>
    /// Typin argument binding exception.
    /// </summary>
    public abstract class ArgumentBindingException : TypinException
    {
        /// <summary>
        /// Argument schema or null.
        /// </summary>
        public ArgumentSchema? ArgumentSchema { get; }

        /// <summary>
        /// Input.
        /// </summary>
        public CommandInput Input { get; }

        /// <summary>
        /// Initializes an instance of <see cref="ArgumentBindingException"/>.
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="input"></param>
        protected ArgumentBindingException(ArgumentSchema? schema, CommandInput input)
        {
            ArgumentSchema = schema;
            Input = input;
        }

        /// <summary>
        /// Initializes an instance of <see cref="ArgumentBindingException"/>.
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="input"></param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        protected ArgumentBindingException(ArgumentSchema? schema, CommandInput input, string? message) : base(message)
        {
            ArgumentSchema = schema;
            Input = input;
        }

        /// <summary>
        /// Initializes an instance of <see cref="ArgumentBindingException"/>.
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="input"></param>
        /// <param name="info"><see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context"><see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected ArgumentBindingException(ArgumentSchema? schema, CommandInput input, SerializationInfo info, StreamingContext context) : base(info, context)
        {
            ArgumentSchema = schema;
            Input = input;
        }

        /// <summary>
        /// Initializes an instance of <see cref="ArgumentBindingException"/>.
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="input"></param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        protected ArgumentBindingException(ArgumentSchema? schema, CommandInput input, string? message, Exception? innerException) : base(message, innerException)
        {
            ArgumentSchema = schema;
            Input = input;
        }
    }
}