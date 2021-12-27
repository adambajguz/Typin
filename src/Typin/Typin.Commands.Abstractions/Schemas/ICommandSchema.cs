namespace Typin.Commands.Schemas
{
    using System;
    using Typin.Models.Schemas;

    /// <summary>
    /// Command schema.
    /// </summary>
    public interface ICommandSchema : ISchema
    {
        /// <summary>
        /// Command name.
        /// If the name is not set, the command is treated as a default command, i.e. the one that gets executed when the user
        /// does not specify a command name in the arguments.
        /// All commands in an application must have different names. Likewise, only one command without a name is allowed.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Whether command is a default command.
        /// </summary>
        bool IsDefault { get; }

        /// <summary>
        /// Whether command is a dynamic command.
        /// </summary>
        bool IsDynamic { get; }

        /// <summary>
        /// Command description, which is used in help text.
        /// </summary>
        string? Description { get; }

        /// <summary>
        /// Command handler type.
        /// </summary>
        Type Handler { get; }

        /// <summary>
        /// Command model schema.
        /// </summary>
        IModelSchema Model { get; }

        ///// <summary>
        ///// Command manual text, which is used in help text.
        ///// </summary>
        //string? Manual { get; }

        ///// <summary>
        ///// List of CLI mode types, in which the command can be executed.
        ///// If null (default) or empty, command can be executed in every registered mode in the app.
        ///// </summary>
        //IReadOnlyCollection<Type>? SupportedModes { get; }

        ///// <summary>
        ///// List of CLI mode types, in which the command cannot be executed.
        ///// If null (default) or empty, command can be executed in every registered mode in the app.
        ///// </summary>
        //IReadOnlyCollection<Type>? ExcludedModes { get; }

        ///// <summary>
        ///// Whether command can be executed in mode <typeparamref name="T"/>.
        ///// </summary>
        //bool CanBeExecutedInMode<T>()
        //    where T : ICliMode;

        ///// <summary>
        ///// Whether command can be executed in mode provided in parameter.
        ///// </summary>
        //bool CanBeExecutedInMode(Type type);

        ///// <summary>
        ///// Whether command has mode restrictions.
        ///// </summary>
        //bool HasModeRestrictions();
    }
}