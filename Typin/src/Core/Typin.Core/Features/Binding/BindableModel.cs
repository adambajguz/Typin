namespace Typin.Features.Binding
{
    using Typin.Models;
    using Typin.Models.Schemas;

    /// <summary>
    /// Bindable model that encapsulated <see cref="IModelSchema"/> and additional metadata.
    /// </summary>
    public sealed class BindableModel
    {
        /// <summary>
        /// Bindable model directive identifier.
        /// </summary>
        public int DirectiveId { get; }

        /// <summary>
        /// Bindable model schema.
        /// </summary>
        public IModelSchema Schema { get; }

        /// <summary>
        /// Bindable model instance.
        /// </summary>
        public IModel Instance { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="BindableModel"/>.
        /// </summary>
        /// <param name="directiveId"></param>
        /// <param name="schema"></param>
        /// <param name="instance"></param>
        public BindableModel(int directiveId, IModelSchema schema, IModel instance)
        {
            DirectiveId = directiveId;
            Schema = schema;
            Instance = instance;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return base.ToString() +
                " | " +
                $"{nameof(DirectiveId)} = {DirectiveId}, " +
                $"{nameof(Schema)} = {{{Schema}}}, " +
                $"{nameof(Instance)} = {Instance}";
        }
    }
}
