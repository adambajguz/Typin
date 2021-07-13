namespace Typin.Hosting
{
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// CLI builder.
    /// </summary>
    public interface ICliBuilder
    {
        /// <summary>
        /// Services collection.
        /// </summary>
        IServiceCollection Services { get; }
    }
}
