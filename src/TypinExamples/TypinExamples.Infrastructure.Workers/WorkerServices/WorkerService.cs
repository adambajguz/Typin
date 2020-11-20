namespace TypinExamples.Workers.Services
{
    using System;
    using BlazorWorker.WorkerCore;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using TypinExamples.Core;
    using TypinExamples.Domain.Models;

    public sealed class WorkerService : IDisposable
    {
        private readonly IWorkerMessageService _messageService;
        private readonly ServiceProvider _provider;

        public WorkerService(IWorkerMessageService messageService)
        {
            _messageService = messageService;
            _messageService.IncomingMessage += OnIncomingMessageFromMainToWorker;

            _provider = BuildDIContainer();
        }

        private ServiceProvider BuildDIContainer()
        {
            ServiceCollection services = new();

            services.ConfigureWorkerCoreServices();

            return services.BuildServiceProvider();
        }

        private void OnIncomingMessageFromMainToWorker(object? sender, string e)
        {
            WorkerMessageModel model = JsonConvert.DeserializeObject<WorkerMessageModel>(e);
            IMediator mediator = _provider.GetRequiredService<IMediator>();
        }

        public string Execute(string data)
        {
            Console.WriteLine(data);
            WorkerMessageModel deserialized = JsonConvert.DeserializeObject<WorkerMessageModel>(data);

            return JsonConvert.SerializeObject(Execute(deserialized));
        }

        public WorkerMessageModel Execute(WorkerMessageModel data)
        {
            IMediator mediator = _provider.GetRequiredService<IMediator>();

            return new WorkerMessageModel();
        }

        public void Dispose()
        {
            _provider.Dispose();
        }
    }
}
