namespace SimpleAppExample.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Commands.Attributes;
    using Typin.Models;
    using Typin.Models.Builders;

    public sealed class ConfigureModelsFromAttributes : IConfigureModel
    {
        public ValueTask ConfigureAsync(IModelBuilder builder, CancellationToken cancellationToken)
        {
            builder.FromAttributes(); //TODO: called twice

            return default;
        }
    }

    public sealed class ConfigureModelsFromAttributes<TModel> : IConfigureModel<TModel>
        where TModel : class, IModel
    {
        public ValueTask ConfigureAsync(IModelBuilder<TModel> builder, CancellationToken cancellationToken)
        {
            builder.FromAttributes();

            return default;
        }
    }

}
