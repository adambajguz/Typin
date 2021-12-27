namespace Typin.Commands.Builders
{
    using System;
    using Typin.Commands;
    using Typin.Commands.Schemas;
    using Typin.Models.Builders;
    using Typin.Models.Schemas;

    /// <summary>
    /// Command builder.
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    public interface ICommandBuilder<TCommand> : IBuilder<ICommandSchema>, IManageExtensions<ICommandBuilder<TCommand>>
        where TCommand : class, ICommand
    {
        /// <summary>
        /// Model schema.
        /// </summary>
        public IModelSchema Model { get; }

        /// <summary>
        /// Sets parameter name to default (empty name).
        /// </summary>
        /// <returns></returns>
        public ICommandBuilder<TCommand> DefaultName()
        {
            return Name(string.Empty);
        }

        /// <summary>
        /// Sets parameter description to default (no description).
        /// </summary>
        /// <returns></returns>
        public ICommandBuilder<TCommand> DefaultDescription()
        {
            return Description(null);
        }

        /// <summary>
        /// Configures a command name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ICommandBuilder<TCommand> Name(string name);

        /// <summary>
        /// Sets command description.
        /// When null is passed a value will be set to default (no description).
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        ICommandBuilder<TCommand> Description(string? description);

        /// <summary>
        /// Configures a command handler.
        /// </summary>
        /// <typeparam name="THandler"></typeparam>
        /// <returns></returns>
        ICommandBuilder<TCommand> Handler<THandler>()
            where THandler : class, ICommandHandler<TCommand>;

        /// <summary>
        /// Configures a command handler.
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        ICommandBuilder<TCommand> Handler(Type handler);
    }
}