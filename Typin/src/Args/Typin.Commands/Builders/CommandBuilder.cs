namespace Typin.Commands.Builders
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Typin.Commands;
    using Typin.Commands.Schemas;
    using Typin.Models.Schemas;
    using Typin.Schemas.Builders;
    using Typin.Schemas.Collections;

    /// <summary>
    /// Base command builder.
    /// </summary>
    public sealed class CommandBuilder<TModel> : ICommandBuilder<TModel>, ICommandBuilder
        where TModel : class, ICommand
    {
        private bool _built;

        private Type? _handler;

        ICommandBuilder ISelf<ICommandBuilder>.Self => this;
        ICommandBuilder<TModel> ISelf<ICommandBuilder<TModel>>.Self => this;

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

                if (!ICommandHandler<TModel>.IsValidType(value, Model.Type))
                {
                    throw new ArgumentException($"'{value}' is not a valid handler for model of type '{Model.Type}'.");
                }

                _handler = value;
            }
        }

        /// <inheritdoc/>
        public IExtensionsCollection Extensions { get; } = new ExtensionsCollection();

        /// <inheritdoc/>
        public IAliasCollection Aliases { get; } = new AliasCollection();

        /// <summary>
        /// Initializes a new instance of <see cref="CommandBuilder{TReturn}"/>.
        /// </summary>
        public CommandBuilder(IModelSchema model)
        {
            Model = model;

            if (!ICommand.IsValidType(model.Type))
            {
                throw new ArgumentException($"Model schema does not represent a '{typeof(ICommand)}' model.", nameof(model));
            }
        }

        /// <inheritdoc/>
        public ICommandBuilder<TModel> ManageExtensions(Action<IExtensionsCollection> action)
        {
            action(Extensions);

            return this;
        }

        /// <inheritdoc/>
        public ICommandBuilder<TModel> ManageAliases(Action<IAliasCollection> action)
        {
            action(Aliases);

            return this;
        }

        /// <inheritdoc/>
        public ICommandBuilder<TModel> UseDescription(string? description)
        {
            Description = description;

            return this;
        }

        /// <inheritdoc/>
        public ICommandBuilder<TModel> UseHandler(Type handler)
        {
            Handler = handler;

            return this;
        }

        /// <inheritdoc/>
        public ICommandSchema Build()
        {
            if (Handler is null)
            {
                Type[] handlers = Model.Type.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(ICommandHandler.IsValidType)
                    .Take(2)
                    .ToArray();

                if (handlers.Length == 1)
                {
                    Handler = handlers[0];
                }
            }

            CommandSchema schema = new(false,
                                       Aliases,
                                       Description,
                                       Model,
                                       Handler ?? throw new InvalidOperationException($"Command handler must be set for command '{Aliases}' ({Model}) when there are multiple or no model-nested handlers."),
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
                throw new InvalidOperationException($"Command was '{Aliases}' ({Model})' already built.");
            }

            _built = true;
        }

        ICommandBuilder IBaseCommandBuilder<ICommandBuilder>.UseDescription(string? description)
        {
            UseDescription(description);

            return this;
        }

        ICommandBuilder IBaseCommandBuilder<ICommandBuilder>.UseHandler(Type handler)
        {
            UseHandler(handler);

            return this;
        }

        ICommandBuilder IBaseCommandBuilder<ICommandBuilder>.UseHandler<THandler>()
        {
            UseHandler(typeof(THandler));

            return this;
        }

        ICommandBuilder<TModel> IBaseCommandBuilder<ICommandBuilder<TModel>>.UseHandler<THandler>()
        {
            UseHandler(typeof(THandler));

            return this;
        }

        ICommandBuilder<TModel> ICommandBuilder<TModel>.UseHandler<THandler>()
        {
            UseHandler(typeof(THandler));

            return this;
        }

        ICommandBuilder IManageExtensions<ICommandBuilder>.ManageExtensions(Action<IExtensionsCollection> action)
        {
            ManageExtensions(action);

            return this;
        }

        ICommandBuilder IManageAliases<ICommandBuilder>.ManageAliases(Action<IAliasCollection> action)
        {
            ManageAliases(action);

            return this;
        }
    }
}
