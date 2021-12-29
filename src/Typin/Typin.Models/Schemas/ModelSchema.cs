namespace Typin.Models.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Typin.Models;
    using Typin.Schemas.Collections;

    /// <summary>
    /// Stores a bindable model schema.
    /// </summary>
    public class ModelSchema : IModelSchema
    {
        /// <inheritdoc/>
        public Type Type { get; }

        /// <inheritdoc/>
        public IEnumerable<IArgumentSchema> Arguments
        {
            get
            {
                foreach (IParameterSchema parameter in Parameters)
                {
                    yield return parameter;
                }

                foreach (IOptionSchema option in Options)
                {
                    yield return option;
                }
                foreach (IOptionSchema option in Options)
                {
                    yield return option;
                }
            }
        }

        /// <inheritdoc/>
        public IReadOnlyList<IParameterSchema> Parameters { get; }

        /// <inheritdoc/>
        public IReadOnlyList<IOptionSchema> Options { get; }

        /// <inheritdoc/>
        public IReadOnlyList<IOptionSchema> RequiredOptions { get; }

        /// <inheritdoc/>
        public IExtensionsCollection Extensions { get; }

        /// <summary>
        /// Initializes an instance of <see cref="ModelSchema"/>.
        /// </summary>
        public ModelSchema(Type type,
                           IReadOnlyList<IParameterSchema> parameters,
                           IReadOnlyList<IOptionSchema> options,
                           IReadOnlyList<IOptionSchema> requiredOptions,
                           IExtensionsCollection extensions)
        {
            Type = type;
            Parameters = parameters;
            Options = options;
            RequiredOptions = requiredOptions;
            Extensions = extensions;
        }

        /// <summary>
        /// Returns dictionary of arguments and its values.
        /// </summary>
        public IReadOnlyDictionary<IArgumentSchema, object?> GetArgumentValues(IModel instance)
        {
            Dictionary<IArgumentSchema, object?> result = new();

            foreach (IArgumentSchema argument in Arguments)
            {
                object? value = argument.Bindable.GetValue(instance);
                result[argument] = value;
            }

            return result;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return base.ToString() +
                " | " +
                $"{nameof(Type)} = {Type}";
        }
    }
}