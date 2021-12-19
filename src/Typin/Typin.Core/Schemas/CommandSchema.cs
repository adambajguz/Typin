namespace Typin.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Typin.Internal.Exceptions;

    /// <summary>
    /// Stores command schema.
    /// </summary>
    public class CommandSchema : BaseCommandSchema
    {
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
        /// Whether command is a dynamic command.
        /// </summary>
        public bool IsDynamic { get; }

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
        /// Initializes an instance of <see cref="CommandSchema"/>.
        /// </summary>
        public CommandSchema(Type type,
                             bool isDynamic,
                             string? name,
                             string? description,
                             string? manual,
                             IReadOnlyList<Type>? supportedModes,
                             IReadOnlyList<Type>? excludedModes,
                             IReadOnlyList<ParameterSchema> parameters,
                             IReadOnlyList<OptionSchema> options) :
            base(type, parameters, options)
        {
            IsDynamic = isDynamic;
            Name = name;
            Description = description;
            Manual = manual;
            SupportedModes = supportedModes?.ToHashSet(); //TODO: only use empty
            ExcludedModes = excludedModes?.ToHashSet(); //TODO: only use empty
        }

        /// <summary>
        /// Initializes an instance of <see cref="CommandSchema"/>.
        /// </summary>
        public CommandSchema(BaseCommandSchema baseCommandSchema,
                             bool isDynamic,
                             string? name,
                             string? description,
                             string? manual,
                             IReadOnlyList<Type>? supportedModes,
                             IReadOnlyList<Type>? excludedModes) :
            base(baseCommandSchema.Type, baseCommandSchema.Parameters, baseCommandSchema.Options)
        {
            IsDynamic = isDynamic;
            Name = name;
            Description = description;
            Manual = manual;
            SupportedModes = supportedModes?.ToHashSet(); //TODO: only use empty
            ExcludedModes = excludedModes?.ToHashSet(); //TODO: only use empty
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
            {
                throw AttributesExceptions.InvalidModeType(type);
            }

            if (!HasModeRestrictions())
            {
                return true;
            }

            if (SupportedModes is not null && !SupportedModes!.Contains(type))
            {
                return false;
            }

            if (ExcludedModes is not null && ExcludedModes.Contains(type))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Whether command has mode restrictions.
        /// </summary>
        public bool HasModeRestrictions()
        {
            return (SupportedModes?.Count ?? 0) > 0 || (ExcludedModes?.Count ?? 0) > 0;
        }
        /// <inheritdoc/>
        public override string ToString()
        {
            StringBuilder buffer = new();

            // Type
            buffer.Append(Type.FullName ?? Type.Name);

            // Name
            buffer.Append(' ')
                  .Append('(')
                  .Append(IsDefault ? "<default command>" : $"'{Name}'")
                  .Append(')');

            return buffer.ToString();
        }
    }
}