namespace Typin.Descriptors
{
    using System;

    /// <summary>
    /// Dynamic command descriptor.
    /// </summary>
    public interface IDynamicCommandDescriptor
    {

    }

    /// <summary>
    /// Command descriptor.
    /// </summary>
    public interface ICommandDescriptor : IDescriptor
    {
        /// <summary>
        /// Command name.
        /// If the name is not set, the command is treated as a default command, i.e. the one that gets executed when the user
        /// does not specify a command name in the arguments.
        /// All commands in an application must have different names. Likewise, only one command without a name is allowed.
        /// </summary>
        public string? Name { get; }

        /// <summary>
        /// Command description, which is used in help text.
        /// </summary>
        public string? Description { get; init; }

        /// <summary>
        /// Command manual text, which is used in help text.
        /// </summary>
        public string? Manual { get; init; }

        /// <summary>
        /// List of CLI mode types, in which the command can be executed.
        /// If null (default) or empty, command can be executed in every registered mode in the app.
        /// </summary>
        public Type[]? SupportedModes { get; init; }

        /// <summary>
        /// List of CLI mode types, in which the command cannot be executed.
        /// If null (default) or empty, command can be executed in every registered mode in the app.
        /// </summary>
        public Type[]? ExcludedModes { get; init; }
    }
}