namespace Typin.Hosting
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Typin.Hosting.Components;

    /// <summary>
    /// Typin command line builder.
    /// </summary>
    public interface ICliBuilder : IScannableBuilder
    {
        /// <summary>
        /// Whether this is a subsequent builder call.
        /// </summary>
        bool SubsequentCall { get; }

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
    }
}