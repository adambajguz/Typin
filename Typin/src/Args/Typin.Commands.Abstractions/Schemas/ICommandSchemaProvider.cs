namespace Typin.Commands.Schemas
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Commands.Collections;

    /// <summary>
    /// Represents a command schema provider.
    /// </summary>
    public interface ICommandSchemaProvider
    {
        /// <summary>
        /// Command schemas.
        /// </summary>
        ICommandSchemaCollection Schemas { get; }

        /// <summary>
        /// Reload schemas.
        /// </summary>
        /// <returns></returns>
        Task ReloadAsync(CancellationToken cancellationToken = default);
    }
}
