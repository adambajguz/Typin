namespace Typin.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using Typin.Internal.Exceptions;

    /// <summary>
    /// Stores command schema.
    /// </summary>
    public class CommandSchema
    {
        /// <summary>
        /// Command type.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Command name.
        /// If the name is not set, the command is treated as a default command, i.e. the one that gets executed when the user
        /// does not specify a command name in the arguments.
        /// All commands in an application must have different names. Likewise, only one command without a name is allowed.
        /// </summary>
        public string? Name { get; }

        /// <summary>
        /// Whether command is a default command.
        /// </summary>
        public bool IsDefault => string.IsNullOrWhiteSpace(Name);

        /// <summary>
        /// Command description, which is used in help text.
        /// </summary>
        public string? Description { get; }

        /// <summary>
        /// Command manual text, which is used in help text.
        /// </summary>
        public string? Manual { get; }

        /// <summary>
        /// List of CLI mode types, in which the command can be executed.
        /// If null (default) or empty, command can be executed in every registered mode in the app.
        /// </summary>
        public IReadOnlyCollection<Type>? SupportedModes { get; }

        /// <summary>
        /// List of CLI mode types, in which the command cannot be executed.
        /// If null (default) or empty, command can be executed in every registered mode in the app.
        /// </summary>
        public IReadOnlyCollection<Type>? ExcludedModes { get; }

        /// <summary>
        /// List of ordered parameters.
        /// </summary>
        public IReadOnlyList<CommandParameterSchema> Parameters { get; }

        /// <summary>
        /// List of options.
        /// </summary>
        public IReadOnlyList<CommandOptionSchema> Options { get; }

        /// <summary>
        /// Whether help option is available for this command.
        /// </summary>
        public bool IsHelpOptionAvailable => Options.Contains(CommandOptionSchema.HelpOption);

        /// <summary>
        /// Whether version option is available for this command.
        /// </summary>
        public bool IsVersionOptionAvailable => Options.Contains(CommandOptionSchema.VersionOption);

        /// <summary>
        /// Initializes an instance of <see cref="CommandSchema"/>.
        /// </summary>
        public CommandSchema(Type type,
                             string? name,
                             string? description,
                             string? manual,
                             Type[]? supportedModes,
                             Type[]? excludedModes,
                             IReadOnlyList<CommandParameterSchema> parameters,
                             IReadOnlyList<CommandOptionSchema> options)
        {
            Type = type;
            Name = name;
            Description = description;
            Manual = manual;
            SupportedModes = supportedModes?.ToHashSet();
            ExcludedModes = excludedModes?.ToHashSet();
            Parameters = parameters;
            Options = options;
        }

        /// <summary>
        /// Whether command can be executed in mode T.
        /// </summary>
        public bool CanBeExecutedInMode<T>()
            where T : ICliMode
        {
            return CanBeExecutedInMode(typeof(T));
        }

        /// <summary>
        /// Whether command can be executed in mode provided in parameter.
        /// </summary>
        public bool CanBeExecutedInMode(Type type)
        {
            if (!KnownTypesHelpers.IsCliModeType(type))
                throw AttributesExceptions.InvalidModeType(type);

            if (!HasModeRestrictions())
                return true;

            if (SupportedModes != null && !SupportedModes!.Contains(type))
                return false;

            if (ExcludedModes != null && ExcludedModes!.Contains(type))
                return false;

            return true;
        }

        /// <summary>
        /// Whether command has mode restrictions.
        /// </summary>
        public bool HasModeRestrictions()
        {
            return (SupportedModes?.Count ?? 0) > 0 || (ExcludedModes?.Count ?? 0) > 0;
        }

        /// <summary>
        /// Enumerates through parameters and options.
        /// </summary>
        public IEnumerable<ArgumentSchema> GetArguments()
        {
            foreach (CommandParameterSchema parameter in Parameters)
                yield return parameter;

            foreach (CommandOptionSchema option in Options)
                yield return option;
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
                if (argument.Property is null)
                    continue;

                object? value = argument.Property.GetValue(instance);
                result[argument] = value;
            }

            return result;
        }

        internal string GetInternalDisplayString()
        {
            StringBuilder buffer = new();

            // Type
            buffer.Append(Type.FullName);

            // Name
            buffer.Append(' ')
                  .Append('(')
                  .Append(IsDefault ? "<default command>" : $"'{Name}'")
                  .Append(')');

            return buffer.ToString();
        }

        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            return GetInternalDisplayString();
        }
    }
}