namespace Typin.Models.Schemas
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Models.Collections;

    /// <summary>
    /// Represents a model schema provider.
    /// </summary>
    public interface IModelSchemaProvider
    {
        /// <summary>
        /// Model schemas.
        /// </summary>
        IModelSchemaCollection Schemas { get; }

        /// <summary>
        /// Reload schemas.
        /// </summary>
        /// <returns></returns>
        Task ReloadAsync(CancellationToken cancellationToken = default);
    }
}
