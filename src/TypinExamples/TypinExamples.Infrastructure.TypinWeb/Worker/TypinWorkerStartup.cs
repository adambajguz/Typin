namespace TypinExamples.Application.Worker
{
    using Microsoft.Extensions.DependencyInjection;
    using TypinExamples.Application.Handlers.Commands;
    using TypinExamples.Application.Services;
    using TypinExamples.Infrastructure.TypinWeb.Handlers;
    using TypinExamples.Infrastructure.TypinWeb.Services;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;

    public class TypinWorkerStartup : IWorkerStartup
    {
        public void Configure(IWorkerConfigurationBuilder builder)
        {
            builder.UseLongRunningProgram()
                   .RegisterCommandHandler<RunExampleCommand, WorkerRunExampleHandler, RunExampleResult>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<TimerService>()
                    .AddTransient<IWebExampleInvokerService, WebExampleInvokerService>();
        }
    }
}
