namespace Typin.Models
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Models.Builders;

    /// <summary>
    /// Represents an object that configures a model <typeparamref name="TModel"/>.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public interface IConfigureModel<TModel>
        where TModel : class, IModel
    {
        /// <summary>
        /// Configure model using a <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        ValueTask ConfigureAsync(IModelBuilder<TModel> builder, CancellationToken cancellationToken);
    }
}
