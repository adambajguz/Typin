﻿namespace Typin.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;

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
        /// List of CLI mode types, in which the command can be executed.
        /// If null (default) or empty, command can be executed in every registered mode in the app.
        /// </summary>
        public IReadOnlyCollection<Type>? SupportedModes { get; }

        /// <summary>
        /// Initializes an instance of <see cref="DirectiveSchema"/>.
        /// </summary>
        public DirectiveSchema(Type type,
                               string name,
                               string? description,
                               Type[]? supportedModes)
        {
            Type = type;
            Name = name;
            Description = description;
            SupportedModes = supportedModes?.ToHashSet();
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
            if ((SupportedModes?.Count ?? 0) == 0)
                return true;

            return SupportedModes!.Contains(type);
        }

        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            var buffer = new StringBuilder();

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