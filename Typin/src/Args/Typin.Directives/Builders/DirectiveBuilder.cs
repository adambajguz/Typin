namespace Typin.Directives.Builders
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Typin.Directives;
    using Typin.Directives.Schemas;
    using Typin.Models.Schemas;
    using Typin.Schemas.Builders;
    using Typin.Schemas.Collections;

    /// <summary>
    /// Directive builder for model <typeparamref name="TModel"/>.
    /// </summary>
    public sealed class DirectiveBuilder<TModel> : IDirectiveBuilder<TModel>, IDirectiveBuilder
        where TModel : class, IDirective
    {
        private bool _built;

        private Type? _handler;

        IDirectiveBuilder ISelf<IDirectiveBuilder>.Self => this;
        IDirectiveBuilder<TModel> ISelf<IDirectiveBuilder<TModel>>.Self => this;

        /// <inheritdoc/>
        public IModelSchema Model { get; }

        /// <inheritdoc/>
        public Type ModelType => Model.Type;

        /// <inheritdoc/>
        public string? Description { get; set; }

        /// <summary>
        /// Handler type.
        /// </summary>
        public Type? Handler
        {
            get => _handler;
            set
            {
                if (value is null)
                {
                    _handler = null;

                    return;
                }

                if (!IDirectiveHandler<TModel>.IsValidType(value))
                {
                    throw new ArgumentException($"'{value}' is not a valid handler for model of type '{Model.Type}'.", nameof(value));
                }

                _handler = value;
            }
        }

        /// <inheritdoc/>
        public IExtensionsCollection Extensions { get; } = new ExtensionsCollection();

        /// <inheritdoc/>
        public IAliasCollection Aliases { get; } = new AliasCollection();

        /// <summary>
        /// Initializes a new instance of <see cref="DirectiveBuilder{TModel}"/>.
        /// </summary>
        public DirectiveBuilder(IModelSchema model)
        {
            Model = model;

            if (!IDirective.IsValidType(model.Type))
            {
                throw new ArgumentException($"Model schema does not represent a '{typeof(IDirective)}' model.", nameof(model));
            }
        }

        /// <inheritdoc/>
        public IDirectiveBuilder<TModel> ManageExtensions(Action<IExtensionsCollection> action)
        {
            action(Extensions);

            return this;
        }

        /// <inheritdoc/>
        public IDirectiveBuilder<TModel> ManageAliases(Action<IAliasCollection> action)
        {
            action(Aliases);

            return this;
        }

        /// <inheritdoc/>
        public IDirectiveBuilder<TModel> UseDescription(string? description)
        {
            Description = description;

            return this;
        }

        /// <inheritdoc/>
        public IDirectiveBuilder<TModel> UseHandler(Type handler)
        {
            Handler = handler;

            return this;
        }

        /// <inheritdoc/>
        public IDirectiveSchema Build()
        {
            if (Handler is null)
            {
                Type[] handlers = Model.Type.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(IDirectiveHandler.IsValidType)
                    .Take(2)
                    .ToArray();

                if (handlers.Length == 1)
                {
                    Handler = handlers[0];
                }
            }

            DirectiveSchema schema = new(Aliases,
                                         Description,
                                         Model,
                                         Handler ?? throw new InvalidOperationException($"Directive handler must be set for directive '{Aliases}' ({Model}) when there are multiple or no model-nested handlers."),
                                         Extensions);

            EnsureBuiltOnce(); // Call before return to allow rebuild on exception.
            return schema;
        }

        /// <summary>
        /// Throws <see cref="InvalidOperationException"/> when built more then once.
        /// It is recommended to call this method just before return to allow rebuild on exception.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        private void EnsureBuiltOnce()
        {
            if (_built)
            {
                throw new InvalidOperationException($"Directive was '{Aliases}' ({Model})' already built.");
            }

            _built = true;
        }

        IDirectiveBuilder IBaseDirectiveBuilder<IDirectiveBuilder>.UseDescription(string? description)
        {
            UseDescription(description);

            return this;
        }

        IDirectiveBuilder IBaseDirectiveBuilder<IDirectiveBuilder>.UseHandler(Type handler)
        {
            UseHandler(handler);

            return this;
        }

        IDirectiveBuilder IBaseDirectiveBuilder<IDirectiveBuilder>.UseHandler<THandler>()
        {
            UseHandler(typeof(THandler));

            return this;
        }


        IDirectiveBuilder<TModel> IBaseDirectiveBuilder<IDirectiveBuilder<TModel>>.UseHandler<THandler>()
        {
            UseHandler(typeof(THandler));

            return this;
        }

        IDirectiveBuilder<TModel> IDirectiveBuilder<TModel>.UseHandler<THandler>()
        {
            UseHandler(typeof(THandler));

            return this;
        }

        IDirectiveBuilder IManageExtensions<IDirectiveBuilder>.ManageExtensions(Action<IExtensionsCollection> action)
        {
            ManageExtensions(action);

            return this;
        }

        IDirectiveBuilder IManageAliases<IDirectiveBuilder>.ManageAliases(Action<IAliasCollection> action)
        {
            ManageAliases(action);

            return this;
        }
    }
}
