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
        public BindableModel(IModelSchema schema, IModel instance)
        {
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
