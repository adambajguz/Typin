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
        /// <param name="factory"></param>
        /// <param name="scanner"></param>
        /// <returns></returns>
        ICliBuilder GetOrAddScanner<TComponent>(Func<IServiceCollection, IScanner<TComponent>> factory, Action<IScanner<TComponent>> scanner)
            where TComponent : class;

        /// <summary>
        /// Get or add CLI component scanner.
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <param name="factory"></param>
        /// <param name="scanner"></param>
        /// <returns></returns>
        ICliBuilder GetOrAddScanner<TComponent>(Func<ICliBuilder, IScanner<TComponent>> factory, Action<IScanner<TComponent>> scanner)
            where TComponent : class;
    }
}