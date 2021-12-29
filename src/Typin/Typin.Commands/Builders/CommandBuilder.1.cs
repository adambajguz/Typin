namespace Typin.Commands.Builders
{
    using System;
    using Typin.Commands;
    using Typin.Models.Builders;
    using Typin.Models.Schemas;
    using Typin.Schemas.Collections;

    /// <summary>
    /// Command builder.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class CommandBuilder<TModel> : CommandBuilder, ICommandBuilder<TModel>
        where TModel : class, ICommand
    {
        /// <summary>
        /// Initializes a new instance of <see cref="CommandBuilder{TModel}"/>.
        /// </summary>
        public CommandBuilder(IModelSchema model) :
            base(model)
        {

        }
        ICommandBuilder<TModel> IManageExtensions<ICommandBuilder<TModel>>.ManageExtensions(Action<IExtensionsCollection> action)
        {
            action(Extensions);

            return this;
        }

        ICommandBuilder<TModel> ICommandBuilder<TModel>.Name(string name)
        {
            Name(name);

            return this;
        }

        ICommandBuilder<TModel> ICommandBuilder<TModel>.Description(string? description)
        {
            Description(description);

            return this;
        }

        ICommandBuilder<TModel> ICommandBuilder<TModel>.Handler<THandler>()
        {
            HandlerType = typeof(THandler);

            return this;
        }

        ICommandBuilder<TModel> ICommandBuilder<TModel>.Handler(Type handler)
        {
            if (!ICommandHandler<TModel>.IsValidType(handler))
            {
                throw new ArgumentException($"'{handler}' is not a valid handler for model of type '{Model.Type}'.");
            }

            HandlerType = handler;

            return this;
        }
    }
}
