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
        /// Bindable model identifier.
        /// </summary>
        public int Id { get; }

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
        /// <param name="id"></param>
        /// <param name="schema"></param>
        /// <param name="instance"></param>
        public BindableModel(int id, IModelSchema schema, IModel instance)
        {
            Id = id;
            Schema = schema;
            Instance = instance;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return base.ToString() +
                " | " +
                $"{nameof(Schema)} = {Schema}, " +
                $"{nameof(Instance)} = {Instance}";
        }
    }
}
