namespace Typin.Models.Schemas
{
    using System;
    using System.Collections.Generic;
    using Typin.Models;
    using Typin.Schemas;

    /// <summary>
    /// Model schema.
    /// </summary>
    public interface IModelSchema : ISchema
    {
        /// <summary>
        /// Model type.
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Gets an enumerator through parameters and options.
        /// </summary>
        IEnumerable<IArgumentSchema> Arguments { get; }

        /// <summary>
        /// List of ordered parameters.
        /// </summary>
        IReadOnlyList<IParameterSchema> Parameters { get; }

        /// <summary>
        /// List of not required options.
        /// </summary>
        IReadOnlyList<IOptionSchema> Options { get; }

        /// <summary>
        /// A list of required options.
        /// </summary>
        IReadOnlyList<IOptionSchema> RequiredOptions { get; }

        /// <summary>
        /// Returns dictionary of arguments and its values.
        /// </summary>
        IReadOnlyDictionary<IArgumentSchema, object?> GetArgumentValues(IModel instance);
    }
}