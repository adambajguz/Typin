namespace TypinExamples.Shared
{
    using Microsoft.Extensions.DependencyInjection;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;

    public class WebWorkerStartup : IWorkerStartup
    {
        public void Configure(IWorkerConfigurationBuilder builder)
        {
            builder.UseProgram<WebWorkerProgram>()
                   .RegisterCommandHandler<WorkerTestCommand, WorkerTestCommand.Handler>()
                   .RegisterNotificationHandler<WorkerTestNotification, WorkerTestNotification.Handler>();
        }

        public void ConfigureServices(IServiceCollection services)
        {

        }
    }
}
