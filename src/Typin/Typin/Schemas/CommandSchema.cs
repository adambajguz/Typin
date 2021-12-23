namespace Typin.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Typin.Internal.Exceptions;
    using Typin.Models.Binding;
    using Typin.Models.Schemas;

    /// <summary>
    /// Command schema.
    /// </summary>
    public class CommandSchema : ModelSchema, ICommandSchema
    {
        /// <inheritdoc/>
        public string? Name { get; }

        /// <inheritdoc/>
        public bool IsDefault => string.IsNullOrWhiteSpace(Name);

        /// <inheritdoc/>
        public bool IsDynamic { get; }

        /// <inheritdoc/>
        public string? Description { get; }

        /// <inheritdoc/>
        public string? Manual { get; }

        /// <inheritdoc/>
        public IReadOnlyCollection<Type>? SupportedModes { get; }

        /// <inheritdoc/>
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
                             IReadOnlyList<IParameterSchema> parameters,
                             IReadOnlyList<IOptionSchema> options) :
            base(type, parameters, options)
        {
            IsDynamic = isDynamic;
            Name = name;
            Description = description;
            Manual = manual;
            SupportedModes = supportedModes?.ToHashSet(); //TODO: only use empty
            ExcludedModes = excludedModes?.ToHashSet(); //TODO: only use empty
        }

        /// <inheritdoc/>
        public bool CanBeExecutedInMode<T>()
            where T : ICliMode
        {
            return CanBeExecutedInMode(typeof(T));
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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