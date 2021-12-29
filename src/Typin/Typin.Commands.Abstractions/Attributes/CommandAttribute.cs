namespace Typin.Commands.Attributes
{
    using System;

    /// <summary>
    /// Annotates a type that defines a command.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class CommandAttribute : Attribute
    {
        /// <summary>
        /// Command name.
        /// If the name is not set, the command is treated as a default command, i.e. the one that gets executed when the user
        /// does not specify a command name in the arguments.
        /// All commands in an application must have different names. Likewise, only one command without a name is allowed.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Command description, which is used in help text.
        /// </summary>
        public string? Description { get; init; }

        /// <summary>
        /// Command manual text, which is used in help text.
        /// </summary>
        public string? Manual { get; init; }

        /// <summary>
        /// Initializes an instance of <see cref="CommandAttribute"/>.
        /// </summary>
        public CommandAttribute(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        /// <summary>
        /// Initializes an instance of <see cref="CommandAttribute"/>.
        /// </summary>
        public CommandAttribute()
        {
            Name = string.Empty;
        }
    }
}