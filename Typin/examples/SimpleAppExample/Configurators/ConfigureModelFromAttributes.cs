namespace SimpleAppExample.Configurators
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Models;
    using Typin.Models.Attributes;
    using Typin.Models.Builders;

    public sealed class ConfigureModelFromAttributes<TModel> : IConfigureModel<TModel>
        where TModel : class, IModel
    {
        public ValueTask ConfigureAsync(IModelBuilder<TModel> builder, CancellationToken cancellationToken)
        {
            builder.FromAttributes();

            return default;
        }
    }

}
