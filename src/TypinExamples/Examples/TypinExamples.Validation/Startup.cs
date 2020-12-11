namespace TypinExamples.Validation
{
    using Microsoft.Extensions.DependencyInjection;
    using Typin;
    using Typin.Directives;
    using Typin.Modes;
    using TypinExamples.Validation.Middleware;

    public class Startup : ICliStartup
    {
        public void Configure(CliApplicationBuilder app)
        {
            app.AddCommandsFromThisAssembly()
               .AddDirective<PreviewDirective>()
               .UseMiddleware<FluentValidationMiddleware>()
               .UseInteractiveMode();
        }

        public void ConfigureServices(IServiceCollection services)
        {

        }
    }
}
