namespace Typin.Directives.Builders
{
    using Typin.Directives;

    /// <summary>
    /// Directive builder.
    /// </summary>
    public interface IDirectiveBuilder : IBaseDirectiveBuilder<IDirectiveBuilder>
    {

    }

    /// <summary>
    /// Directive builder.
    /// </summary>
    public interface IDirectiveBuilder<TModel> : IBaseDirectiveBuilder<IDirectiveBuilder<TModel>>
        where TModel : class, IDirective
    {
        /// <summary>
        /// Configures a directive handler.
        /// </summary>
        /// <typeparam name="THandler"></typeparam>
        /// <returns></returns>
        new IDirectiveBuilder<TModel> UseHandler<THandler>()
            where THandler : class, IDirectiveHandler<TModel>;
    }
}