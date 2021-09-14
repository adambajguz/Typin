namespace Typin.Exceptions.Resolvers.ParameterResolver
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Typin.Exceptions.Resolvers;
    using Typin.Schemas;

    /// <summary>
    /// Typin parameter resolver exception.
    /// </summary>
    public abstract class ParameterResolverException : ResolverException
    {
        /// <summary>
        /// Command schema.
        /// </summary>
        public BaseCommandSchema? CommandSchema { get; }

        /// <summary>
        /// Invalid parameters.
        /// </summary>
        public IReadOnlyList<ParameterSchema> InvalidParameters { get; }

        /// <summary>
        /// Initializes an instance of <see cref="ParameterResolverException"/>.
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="invalidParameters"></param>
        protected ParameterResolverException(BaseCommandSchema schema, IReadOnlyList<ParameterSchema> invalidParameters)
        {
            CommandSchema = schema;
            InvalidParameters = invalidParameters;
        }

        /// <summary>
        /// Initializes an instance of <see cref="ParameterResolverException"/>.
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="invalidParameters"></param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        protected ParameterResolverException(BaseCommandSchema schema, IReadOnlyList<ParameterSchema> invalidParameters, string? message) : base(message)
        {
            CommandSchema = schema;
            InvalidParameters = invalidParameters;
        }

        /// <summary>
        /// Initializes an instance of <see cref="ParameterResolverException"/>.
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="invalidParameters"></param>
        /// <param name="info"><see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context"><see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected ParameterResolverException(BaseCommandSchema schema, IReadOnlyList<ParameterSchema> invalidParameters, SerializationInfo info, StreamingContext context) : base(info, context)
        {
            CommandSchema = schema;
            InvalidParameters = invalidParameters;
        }

        /// <summary>
        /// Initializes an instance of <see cref="ParameterResolverException"/>.
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="invalidParameters"></param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        protected ParameterResolverException(BaseCommandSchema schema, IReadOnlyList<ParameterSchema> invalidParameters, string? message, Exception? innerException) : base(message, innerException)
        {
            CommandSchema = schema;
            InvalidParameters = invalidParameters;
        }
    }
}