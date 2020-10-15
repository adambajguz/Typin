namespace Typin
{
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Abstraction for Typin framework configuration using startup class.
    /// </summary>
    public interface ICliStartup
    {
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        void ConfigureServices(IServiceCollection services);

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure Typin framework.
        /// </summary>
        void Configure(CliApplicationBuilder app);
    }
}
