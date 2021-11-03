namespace Typin.Hosting
{
    using System;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Typin.Components;

    /// <summary>
    /// Typin command line builder.
    /// </summary>
    public interface ICliBuilder
    {
        /// <summary>
        /// The <see cref="HostBuilderContext"/> initialized by the <see cref="IHost"/>.
        /// </summary>
        HostBuilderContext Context { get; }

        /// <summary>
        /// The <see cref="IHostEnvironment"/> initialized by the <see cref="IHost"/>.
        /// </summary>
        IHostEnvironment Environment { get; }

        /// <summary>
        /// The <see cref="IConfiguration"/> containing the merged configuration of the application and the <see cref="IHost"/>.
        /// </summary>
        IConfiguration Configuration { get; }

        /// <summary>
        /// The <see cref="IServiceCollection"/> initialized by the <see cref="IHost"/>.
        /// </summary>
        IServiceCollection Services { get; }

        /// <summary>
        /// Get or add CLI component scanner.
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <typeparam name="TInterface"></typeparam>
        /// <param name="factory"></param>
        /// <param name="scanner"></param>
        /// <returns></returns>
        ICliBuilder GetOrAddScanner<TComponent, TInterface>(Func<ICliBuilder, TInterface> factory, Action<TInterface> scanner)
            where TComponent : class
            where TInterface : class, IScanner<TComponent>;
    }
}