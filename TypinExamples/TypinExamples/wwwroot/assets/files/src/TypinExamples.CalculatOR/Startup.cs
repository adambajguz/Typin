namespace TypinExamples.CalculatOR
{
    using Microsoft.Extensions.DependencyInjection;
    using Typin;
    using Typin.Directives;
    using TypinExamples.CalculatOR.Services;
    using TypinExamples.Infrastructure.TypinWeb.Commands;

    public class Startup : ICliStartup
    {
        public void Configure(CliApplicationBuilder app)
        {
            app.AddCommandsFromThisAssembly()
               .AddCommand<PipelineCommand>()
               .AddCommand<ServicesCommand>()
               .AddDirective<PreviewDirective>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<OperationEvaluatorService>();
        }
    }
}
