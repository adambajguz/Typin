namespace Typin.Hosting.Components
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Hosting;

    /// <summary>
    /// CLI component collection.
    /// </summary>
    public interface ICliComponentCollection
    {
        /// <summary>
        /// Services collection.
        /// </summary>
        public ICliBuilder CliBuilder { get; }

        /// <summary>
        /// Add a custom block to the application.
        /// </summary>
        public TBlock GetOrAdd<TBlock>(Func<IServiceCollection, TBlock> factory)
             where TBlock : CliComponent;

        /// <summary>
        /// Add a custom block to the application.
        /// </summary>
        public TBlock GetOrAdd<TBlock>(Func<ICliBuilder, TBlock> factory)
             where TBlock : CliComponent;
    }
}
