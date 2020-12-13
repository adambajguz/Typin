namespace TypinExamples.MarioBuilder
{
    using Microsoft.Extensions.DependencyInjection;
    using Typin;
    using Typin.Directives;
    using Typin.Modes;

    public class Startup : ICliStartup
    {
        public void Configure(CliApplicationBuilder app)
        {
            app.AddCommandsFromThisAssembly()
               .AddDirective<PreviewDirective>()
               .UseInteractiveMode(options: (cfg) => cfg.IsAdvancedInputAvailable = false);
        }

        public void ConfigureServices(IServiceCollection services)
        {

        }
    }
}
