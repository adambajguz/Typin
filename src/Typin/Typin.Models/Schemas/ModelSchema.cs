namespace Typin.Models.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Typin.Models;
    using Typin.Models.Collections;

    /// <summary>
    /// Stores a bindable model schema.
    /// </summary>
    public class ModelSchema : IModelSchema
    {
        /// <summary>
        /// Command or dynamic command type.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// List of ordered parameters.
        /// </summary>
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
            }
        }

        /// <summary>
        /// List of ordered parameters.
        /// </summary>
        public IReadOnlyList<IParameterSchema> Parameters { get; }

        /// <summary>
        /// List of required and not required options.
        /// </summary>
        public IReadOnlyList<IOptionSchema> Options { get; }

        /// <summary>
        /// A list of required options.
        /// </summary>
        public IReadOnlyList<IOptionSchema> RequiredOptions { get; }

        /// <inheritdoc/>
        public IExtensionsCollection Extensions { get; }

        /// <summary>
        /// Initializes an instance of <see cref="ModelSchema"/>.
        /// </summary>
        public ModelSchema(Type type,
                           IReadOnlyList<IParameterSchema> parameters,
                           IReadOnlyList<IOptionSchema> options,
                           IExtensionsCollection extensions)
        {
            Type = type;
            Parameters = parameters;
            Options = options;
            RequiredOptions = options.Where(x => x.IsRequired).ToList();
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