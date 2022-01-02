namespace Typin.Directives.Builders
{
    using System;
    using Typin.Directives;
    using Typin.Models.Builders;
    using Typin.Models.Schemas;
    using Typin.Schemas.Collections;

    /// <summary>
    /// Directive builder.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class DirectiveBuilder<TModel> : DirectiveBuilder, IDirectiveBuilder<TModel>
        where TModel : class, IDirective
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DirectiveBuilder{TModel}"/>.
        /// </summary>
        public DirectiveBuilder(IModelSchema model) :
            base(model)
        {

        }

        IDirectiveBuilder<TModel> IManageExtensions<IDirectiveBuilder<TModel>>.ManageExtensions(Action<IExtensionsCollection> action)
        {
            action(Extensions);

            return this;
        }

        IDirectiveBuilder<TModel> IDirectiveBuilder<TModel>.Name(string name)
        {
            Name(name);

            return this;
        }

        IDirectiveBuilder<TModel> IDirectiveBuilder<TModel>.Description(string? description)
        {
            Description(description);

            return this;
        }

        IDirectiveBuilder<TModel> IDirectiveBuilder<TModel>.Handler<THandler>()
        {
            HandlerType = typeof(THandler);

            return this;
        }

        IDirectiveBuilder<TModel> IDirectiveBuilder<TModel>.Handler(Type handler)
        {
            if (!IDirectiveHandler<TModel>.IsValidType(handler))
            {
                throw new ArgumentException($"'{handler}' is not a valid handler for model of type '{Model.Type}'.");
            }

            HandlerType = handler;

            return this;
        }
    }
}
