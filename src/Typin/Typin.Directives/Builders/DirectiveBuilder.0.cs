namespace Typin.Directives.Builders
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Typin.Directives;
    using Typin.Directives.Schemas;
    using Typin.Models.Builders;
    using Typin.Models.Schemas;
    using Typin.Schemas.Collections;

    /// <summary>
    /// Directive builder.
    /// </summary>
    public class DirectiveBuilder : IDirectiveBuilder
    {
        private bool _built;

        private string? _name;
        private string? _description;

        /// <summary>
        /// Handler type.
        /// </summary>
        protected Type? HandlerType { get; set; }

        /// <inheritdoc/>
        public IModelSchema Model { get; }

        /// <summary>
        /// A collection of extensions.
        /// </summary>
        public IExtensionsCollection Extensions { get; } = new ExtensionsCollection();

        /// <summary>
        /// Initializes a new instance of <see cref="DirectiveBuilder{TModel}"/>.
        /// </summary>
        public DirectiveBuilder(IModelSchema model)
        {
            Model = model;

            if (!model.Type.GetInterfaces().Contains(typeof(IDirective)))
            {
                throw new ArgumentException($"Model schema does not represent a '{typeof(IDirective)}' model.", nameof(model));
            }
        }

        IDirectiveBuilder IManageExtensions<IDirectiveBuilder>.ManageExtensions(Action<IExtensionsCollection> action)
        {
            action(Extensions);

            return this;
        }

        /// <inheritdoc/>
        public IDirectiveBuilder Name(string name)
        {
            _name = name.Trim();

            return this;
        }

        /// <inheritdoc/>
        public IDirectiveBuilder Description(string? description)
        {
            _description = description;

            return this;
        }

        /// <inheritdoc/>
        public IDirectiveBuilder Handler<THandler>()
            where THandler : class, IDirectiveHandler
        {
            return Handler(typeof(THandler));
        }

        /// <inheritdoc/>
        public IDirectiveBuilder Handler(Type handler)
        {
            if (!IDirectiveHandler.IsValidType(handler, Model.Type))
            {
                throw new ArgumentException($"'{handler}' is not a valid handler for model of type '{Model.Type}'.");
            }

            HandlerType = handler;

            return this;
        }

        /// <inheritdoc/>
        public IDirectiveSchema Build()
        {
            if (HandlerType is null)
            {
                Type[] handlers = Model.Type.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(IDirectiveHandler.IsValidType)
                    .Take(2)
                    .ToArray();

                if (handlers.Length == 1)
                {
                    HandlerType = handlers[0];
                }
            }

            DirectiveSchema schema = new(_name ?? string.Empty,
                                         _description,
                                         Model,
                                         HandlerType ?? throw new InvalidOperationException($"Directive handler must be set for directive '{_name}' ({Model}) when there are multiple or no model-nested handlers."),
                                         Extensions);

            EnsureBuiltOnce(); // Call before return to allow rebuild on exception.
            return schema;
        }

        /// <summary>
        /// Throws <see cref="InvalidOperationException"/> when built more then once.
        /// It is recommended to call this method just before return to allow rebuild on exception.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        protected void EnsureBuiltOnce()
        {
            if (_built)
            {
                throw new InvalidOperationException($"Directive was '{_name}' ({Model})' already built.");
            }

            _built = true;
        }
    }
}
