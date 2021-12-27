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
    public interface ICommandBuilder : IBuilder<ICommandSchema>, IManageExtensions<ICommandBuilder>
    {
        /// <summary>
        /// Model schema.
        /// </summary>
        public IModelSchema Model { get; }

        /// <summary>
        /// Sets parameter name to default (empty name).
        /// </summary>
        /// <returns></returns>
        public ICommandBuilder DefaultName()
        {
            return Name(string.Empty);
        }

        /// <summary>
        /// Sets parameter description to default (no description).
        /// </summary>
        /// <returns></returns>
        public ICommandBuilder DefaultDescription()
        {
            return Description(null);
        }

        /// <summary>
        /// Configures a command name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ICommandBuilder Name(string name);

        /// <summary>
        /// Sets command description.
        /// When null is passed a value will be set to default (no description).
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        ICommandBuilder Description(string? description);

        /// <summary>
        /// Configures a command handler.
        /// </summary>
        /// <typeparam name="THandler"></typeparam>
        /// <returns></returns>
        ICommandBuilder Handler<THandler>()
            where THandler : class, ICommandHandler;

        /// <summary>
        /// Configures a command handler.
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        ICommandBuilder Handler(Type handler);
    }
}