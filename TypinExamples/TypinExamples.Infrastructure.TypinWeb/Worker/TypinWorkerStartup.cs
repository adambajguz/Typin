﻿namespace TypinExamples.Application.Worker
{
    using Microsoft.Extensions.DependencyInjection;
    using TypinExamples.Application.Handlers.Commands;
    using TypinExamples.Application.Utils;
    using TypinExamples.Infrastructure.TypinWeb.Handlers.Commands;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;

    public class TypinWorkerStartup : IWorkerStartup
    {
        public void Configure(IWorkerConfigurationBuilder builder)
        {
            builder.RegisterCommandHandler<RunExampleCommand, WorkerRunExampleHandler, RunExampleResult>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<SimpleTimer>();
        }
    }
}
