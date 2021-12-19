namespace Typin.Exceptions.Resolvers.OptionResolver
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Typin.Exceptions.Resolvers;
    using Typin.Schemas;

    /// <summary>
    /// Typin option resolver exception.
    /// </summary>
    public abstract class OptionResolverException : ResolverException
    {
        /// <summary>
        /// Command schema.
        /// </summary>
        public BaseCommandSchema? CommandSchema { get; }

        /// <summary>
        /// Invalid options.
        /// </summary>
        public IReadOnlyList<OptionSchema> InvalidOptions { get; }

        /// <summary>
        /// Initializes an instance of <see cref="OptionResolverException"/>.
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="invalidOptions"></param>
        protected OptionResolverException(BaseCommandSchema schema, IReadOnlyList<OptionSchema> invalidOptions)
        {
            CommandSchema = schema;
            InvalidOptions = invalidOptions;
        }

        /// <summary>
        /// Initializes an instance of <see cref="OptionResolverException"/>.
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="invalidOptions"></param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        protected OptionResolverException(BaseCommandSchema schema, IReadOnlyList<OptionSchema> invalidOptions, string? message) : base(message)
        {
            CommandSchema = schema;
            InvalidOptions = invalidOptions;
        }

        /// <summary>
        /// Initializes an instance of <see cref="OptionResolverException"/>.
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="invalidOptions"></param>
        /// <param name="info"><see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context"><see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected OptionResolverException(BaseCommandSchema schema, IReadOnlyList<OptionSchema> invalidOptions, SerializationInfo info, StreamingContext context) : base(info, context)
        {
            CommandSchema = schema;
            InvalidOptions = invalidOptions;
        }

        /// <summary>
        /// Initializes an instance of <see cref="OptionResolverException"/>.
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="invalidOptions"></param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        protected OptionResolverException(BaseCommandSchema schema, IReadOnlyList<OptionSchema> invalidOptions, string? message, Exception? innerException) : base(message, innerException)
        {
            CommandSchema = schema;
            InvalidOptions = invalidOptions;
        }
    }
}