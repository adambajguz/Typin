namespace SimpleAppExample.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Models;
    using Typin.Models.Attributes;
    using Typin.Models.Builders;

    public sealed class ConfigureModelsFromAttributes : IConfigureModel
    {
        public ValueTask ConfigureAsync(IModelBuilder builder, CancellationToken cancellationToken)
        {
            builder.FromAttributes();

            return default;
        }
    }

}
