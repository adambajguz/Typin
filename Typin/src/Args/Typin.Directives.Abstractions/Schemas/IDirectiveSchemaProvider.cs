namespace Typin.Directives.Schemas
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Directives.Collections;

    /// <summary>
    /// Represents a directive schema provider.
    /// </summary>
    public interface IDirectiveSchemaProvider
    {
        /// <summary>
        /// Directive schemas.
        /// </summary>
        IDirectiveSchemaCollection Schemas { get; }

        /// <summary>
        /// Reload schemas.
        /// </summary>
        /// <returns></returns>
        Task ReloadAsync(CancellationToken cancellationToken = default);
    }
}
