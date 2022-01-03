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
    /// <typeparam name="TDirective"></typeparam>
    public interface IDirectiveBuilder<TDirective> : IBuilder<IDirectiveSchema>, IManageExtensions<IDirectiveBuilder<TDirective>>
        where TDirective : class, IDirective
    {
        /// <summary>
        /// Model schema.
        /// </summary>
        public IModelSchema Model { get; }

        /// <summary>
        /// Sets parameter name to default (empty name).
        /// </summary>
        /// <returns></returns>
        public IDirectiveBuilder<TDirective> DefaultName()
        {
            return Name(string.Empty);
        }

        /// <summary>
        /// Sets parameter alias to default (no alias).
        /// </summary>
        /// <returns></returns>
        public IDirectiveBuilder<TDirective> DefaultAlias()
        {
            return Alias(null);
        }

        /// <summary>
        /// Sets parameter description to default (no description).
        /// </summary>
        /// <returns></returns>
        public IDirectiveBuilder<TDirective> DefaultDescription()
        {
            return Description(null);
        }

        /// <summary>
        /// Configures a directive name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IDirectiveBuilder<TDirective> Name(string name);

        /// <summary>
        /// Configures a directive alias.
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        IDirectiveBuilder<TDirective> Alias(string? alias);

        /// <summary>
        /// Sets directive description.
        /// When null is passed a value will be set to default (no description).
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        IDirectiveBuilder<TDirective> Description(string? description);

        /// <summary>
        /// Configures a directive handler.
        /// </summary>
        /// <typeparam name="THandler"></typeparam>
        /// <returns></returns>
        IDirectiveBuilder<TDirective> Handler<THandler>()
            where THandler : class, IDirectiveHandler<TDirective>;

        /// <summary>
        /// Configures a directive handler.
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        IDirectiveBuilder<TDirective> Handler(Type handler);
    }
}