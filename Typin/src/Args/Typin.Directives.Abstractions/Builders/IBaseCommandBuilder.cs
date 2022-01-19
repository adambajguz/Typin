namespace Typin.Directives.Builders
{
    using System;
    using Typin.Directives;
    using Typin.Directives.Schemas;
    using Typin.Models.Schemas;
    using Typin.Schemas.Builders;

    /// <summary>
    /// Directive builder.
    /// </summary>
    public interface IBaseDirectiveBuilder<TSelf> : IBuilder<IDirectiveSchema>,
                                                    IManageExtensions<TSelf>,
                                                    IManageAliases<TSelf>
        where TSelf : class, IBaseDirectiveBuilder<TSelf>
    {
        /// <summary>
        /// Model schema.
        /// </summary>
        IModelSchema Model { get; }

        /// <summary>
        /// Gets or sets a description.
        /// Equivalent to <see cref="UseDescription(string?)"/> and <see cref="UseDefaultDescription"/>.
        /// </summary>
        string? Description { get; set; }

        /// <summary>
        /// Gets or sets a directive handler.
        /// Equivalent to <see cref="UseHandler{THandler}"/> and <see cref="UseHandler(Type)"/>.
        /// </summary>
        Type? Handler { get; set; }

        /// <summary>
        /// Sets parameter description to default (no description).
        /// </summary>
        /// <returns></returns>
        public TSelf UseDefaultDescription()
        {
            return UseDescription(null);
        }

        /// <summary>
        /// Sets directive description.
        /// When null is passed a value will be set to default (no description).
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        TSelf UseDescription(string? description);

        /// <summary>
        /// Configures a directive handler.
        /// </summary>
        /// <typeparam name="THandler"></typeparam>
        /// <returns></returns>
        TSelf UseHandler<THandler>()
            where THandler : class, IDirectiveHandler;

        /// <summary>
        /// Configures a directive handler.
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        TSelf UseHandler(Type handler);
    }
}