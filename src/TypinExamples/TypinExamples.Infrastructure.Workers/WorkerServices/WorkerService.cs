namespace TypinExamples.Workers.Services
{
    using System;
    using System.Threading.Tasks;
    using BlazorWorker.WorkerCore;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using TypinExamples.Core;
    using TypinExamples.Core.Handlers.Workers.Commands;
    using TypinExamples.Core.Handlers.Workers.Notifications;
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

        public async Task<string> Execute(string data)
        {
            Console.WriteLine(data);
            WorkerMessageModel deserialized = JsonConvert.DeserializeObject<WorkerMessageModel>(data);

            WorkerMessageModel result = await Execute(deserialized);

            return JsonConvert.SerializeObject(result);
        }

        public async Task<WorkerMessageModel> Execute(WorkerMessageModel data)
        {
            IMediator mediator = _provider.GetRequiredService<IMediator>();

            if (data.TargetCommandType is not null)
            {
                Type? type = Type.GetType(data.TargetCommandType);

                return await mediator.Send(new WorkerPingCommand());
            }

            if (data.TargetNotificationType is not null)
            {
                Type? type = Type.GetType(data.TargetNotificationType);

                await mediator.Publish(new WorkerPingNotification());
            }

            return WorkerMessageModel.Empty;
        }

        public void Dispose()
        {
            _provider.Dispose();
        }
    }
}
