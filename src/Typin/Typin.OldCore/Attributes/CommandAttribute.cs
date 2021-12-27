namespace Typin.Attributes
{
    using System;
    using Typin.Descriptors;

    /// <summary>
    /// Annotates a type that defines a command.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class CommandAttribute : Attribute, ICommandDescriptor
    {
        /// <inheritdoc/>
        public string? Name { get; }

        /// <inheritdoc/>
        public string? Description { get; init; }

        /// <inheritdoc/>
        public string? Manual { get; init; }

        /// <inheritdoc/>
        public Type[]? SupportedModes { get; init; }

        /// <inheritdoc/>
        public Type[]? ExcludedModes { get; init; }

        /// <summary>
        /// Initializes an instance of <see cref="CommandAttribute"/>.
        /// </summary>
        public CommandAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Initializes an instance of <see cref="CommandAttribute"/>.
        /// </summary>
        public CommandAttribute()
        {

        }
    }
}