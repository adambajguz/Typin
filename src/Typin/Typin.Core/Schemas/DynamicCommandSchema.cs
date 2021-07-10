namespace Typin.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Stores commmand or dynamic command schema.
    /// </summary>
    public class BaseCommandSchema
    {
        /// <summary>
        /// Command or dynamic command type.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// List of ordered parameters.
        /// </summary>
        public IReadOnlyList<ParameterSchema> Parameters { get; }

        /// <summary>
        /// List of options.
        /// </summary>
        public IReadOnlyList<OptionSchema> Options { get; }

        /// <summary>
        /// Whether help option is available for this command.
        /// </summary>
        public bool IsHelpOptionAvailable => Options.Contains(OptionSchema.HelpOption);

        /// <summary>
        /// Whether version option is available for this command.
        /// </summary>
        public bool IsVersionOptionAvailable => Options.Contains(OptionSchema.VersionOption);

        /// <summary>
        /// Initializes an instance of <see cref="CommandSchema"/>.
        /// </summary>
        public BaseCommandSchema(Type type,
                                 IReadOnlyList<ParameterSchema> parameters,
                                 IReadOnlyList<OptionSchema> options)
        {
            Type = type;
            Parameters = parameters;
            Options = options;
        }

        /// <summary>
        /// Enumerates through parameters and options.
        /// </summary>
        public IEnumerable<ArgumentSchema> GetArguments()
        {
            foreach (ParameterSchema parameter in Parameters)
            {
                yield return parameter;
            }

            foreach (OptionSchema option in Options)
            {
                yield return option;
            }
        }

        /// <summary>
        /// Returns dictionary of arguments and its values.
        /// </summary>
        public IReadOnlyDictionary<ArgumentSchema, object?> GetArgumentValues(ICommand instance)
        {
            Dictionary<ArgumentSchema, object?> result = new();

            foreach (ArgumentSchema argument in GetArguments())
            {
                // Skip built-in arguments
                if (argument.Bindable.Kind == BindableArgumentKind.BuiltIn)
                {
                    continue;
                }

                object? value = argument.Bindable.GetValue(instance);
                result[argument] = value;
            }

            return result;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Type.FullName ?? Type.Name;
        }
    }
}