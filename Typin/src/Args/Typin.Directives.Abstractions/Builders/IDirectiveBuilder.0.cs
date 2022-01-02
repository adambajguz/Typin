namespace Typin.Directives.Builders
{
    using System;
    using Typin.Directives;
    using Typin.Directives.Schemas;
    using Typin.Models.Builders;
    using Typin.Models.Schemas;

    /// <summary>
    /// Directive builder.
    /// </summary>
    public interface IDirectiveBuilder : IBuilder<IDirectiveSchema>, IManageExtensions<IDirectiveBuilder>
    {
        /// <summary>
        /// Model schema.
        /// </summary>
        public IModelSchema Model { get; }

        /// <summary>
        /// Sets parameter name to default (empty name).
        /// </summary>
        /// <returns></returns>
        public IDirectiveBuilder DefaultName()
        {
            return Name(string.Empty);
        }

        /// <summary>
        /// Sets parameter description to default (no description).
        /// </summary>
        /// <returns></returns>
        public IDirectiveBuilder DefaultDescription()
        {
            return Description(null);
        }

        /// <summary>
        /// Configures a directive name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IDirectiveBuilder Name(string name);

        /// <summary>
        /// Sets directive description.
        /// When null is passed a value will be set to default (no description).
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        IDirectiveBuilder Description(string? description);

        /// <summary>
        /// Configures a directive handler.
        /// </summary>
        /// <typeparam name="THandler"></typeparam>
        /// <returns></returns>
        IDirectiveBuilder Handler<THandler>()
            where THandler : class, IDirectiveHandler;

        /// <summary>
        /// Configures a directive handler.
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        IDirectiveBuilder Handler(Type handler);
    }
}