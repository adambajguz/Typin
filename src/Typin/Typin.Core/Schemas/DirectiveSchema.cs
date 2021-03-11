namespace Typin.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using Typin.Internal.Exceptions;

    /// <summary>
    /// Stores directive schema.
    /// </summary>
    public class DirectiveSchema
    {
        /// <summary>
        /// Directive type.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Directive name.
        /// All directives in an application must have different names.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Directive description, which is used in help text.
        /// </summary>
        public string? Description { get; }

        /// <summary>
        /// List of CLI mode types, in which the directive can be executed.
        /// If null (default) or empty, directive can be executed in every registered mode in the app.
        /// </summary>
        public IReadOnlyCollection<Type>? SupportedModes { get; }

        /// <summary>
        /// List of CLI mode types, in which the directive cannot be executed.
        /// If null (default) or empty, directive can be executed in every registered mode in the app.
        /// </summary>
        public IReadOnlyCollection<Type>? ExcludedModes { get; }

        /// <summary>
        /// Whether directive is pipelined directive.
        /// </summary>
        public bool IsPipelinedDirective { get; }

        /// <summary>
        /// Initializes an instance of <see cref="DirectiveSchema"/>.
        /// </summary>
        public DirectiveSchema(Type type,
                               string name,
                               string? description,
                               Type[]? supportedModes,
                               Type[]? excludedModes)
        {
            Type = type;
            Name = name;
            Description = description;
            SupportedModes = supportedModes?.ToHashSet();
            ExcludedModes = excludedModes?.ToHashSet();
            IsPipelinedDirective = typeof(IPipelinedDirective).IsAssignableFrom(type);
        }

        /// <summary>
        /// Whether directive can be executed in mode T.
        /// </summary>
        public bool CanBeExecutedInMode<T>()
            where T : ICliMode
        {
            return CanBeExecutedInMode(typeof(T));
        }

        /// <summary>
        /// Whether directive can be executed in mode provided in parameter.
        /// </summary>
        public bool CanBeExecutedInMode(Type type)
        {
            if (!KnownTypesHelpers.IsCliModeType(type))
                throw AttributesExceptions.InvalidModeType(type);

            if (!HasModeRestrictions())
                return true;

            if (SupportedModes is not null && !SupportedModes!.Contains(type))
                return false;

            if (ExcludedModes is not null && ExcludedModes!.Contains(type))
                return false;

            return true;
        }

        /// <summary>
        /// Whether directive has mode restrictions.
        /// </summary>
        public bool HasModeRestrictions()
        {
            return (SupportedModes?.Count ?? 0) > 0 || (ExcludedModes?.Count ?? 0) > 0;
        }

        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            StringBuilder buffer = new();

            // Type
            buffer.Append(Type.FullName);

            // Name
            buffer.Append(' ')
                  .Append('[')
                  .Append(Name)
                  .Append(']');

            return buffer.ToString();
        }
    }
}