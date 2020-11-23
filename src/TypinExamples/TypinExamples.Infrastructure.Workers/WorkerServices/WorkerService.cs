namespace TypinExamples.Infrastructure.Workers.WorkerServices
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using BlazorWorker.WorkerCore;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using TypinExamples.Application;
    using TypinExamples.Application.Services.Workers;
    using TypinExamples.Domain.Extensions;
    using TypinExamples.Domain.Interfaces;
    using TypinExamples.Domain.Models.Workers;
    using TypinExamples.Infrastructure.TypinWeb;
    using TypinExamples.Infrastructure.Workers.Extensions;

    public sealed class WorkerService : IDisposable
    {
        private readonly IWorkerMessageService _messageService;
        private readonly HttpClient _httpClient;
        private readonly ServiceProvider _provider;

        public WorkerService(IWorkerMessageService messageService, HttpClient httpClient)
        {
            _messageService = messageService;
            _httpClient = httpClient;
            _messageService.IncomingMessage += OnIncomingMessageFromMainToWorker;

            _provider = BuildDIContainer();
        }

        private ServiceProvider BuildDIContainer()
        {
            ServiceCollection services = new();

            services.ConfigureWorkerCoreServices();

            services.AddSingleton<ICoreMessageDispatcher, CoreMessageDispatcher>()
                    .AddSingleton(_httpClient)
                    .AddSingleton(_messageService);

            return services.BuildServiceProvider();
        }

        private async void OnIncomingMessageFromMainToWorker(object? sender, string e)
        {
            if (!string.IsNullOrWhiteSpace(e))
                await Dispatch(e);
        }

        public async Task<string> Dispatch(string data)
        {
            WorkerMessage deserialized = JsonConvert.DeserializeObject<WorkerMessage>(data);
            WorkerResult result = await Dispatch(deserialized);

            return JsonConvert.SerializeObject(result);
        }

        private async Task<WorkerResult> Dispatch(WorkerMessage model)
        {
            try
            {
                IMediator mediator = _provider.GetRequiredService<IMediator>();

                if (model.TargetType is string targetType &&
                    model.Data is string data)
                {
                    Type? type = Type.GetType(targetType);

                    if (type is Type t && JsonConvert.DeserializeObject(data, t) is object obj)
                    {
                        if (obj is IWorkerIdentifiable wi)
                            wi.WorkerId = model.WorkerId;

                        if (model.IsNotification)
                        {
                            await mediator.Publish(obj);

                            return WorkerResult.CreateWorkerConfirmation(model.Id, model.WorkerId ?? -1);
                        }
                        else
                        {
                            object? x = await mediator.Send(obj);

                            return x as WorkerResult ?? throw new ApplicationException("Worker command handled by mediator returned invalid return type.");
                        }
                    }
                }

                throw new ApplicationException("Worker was not able to dispach a message.");
            }
            catch (Exception ex)
            {
                ICoreMessageDispatcher coreTaskDispatcher = _provider.GetRequiredService<ICoreMessageDispatcher>();

                WorkerMessage exceptionMessage = this.CreateMessageBuilder()
                                                     .HandleException(model.WorkerId ?? -1, ex)
                                                     .Build();

                await coreTaskDispatcher.DispatchAsync(exceptionMessage);

                return WorkerResult.CreateWorkerException(model.Id, model.WorkerId ?? -1, exceptionMessage);
            }
        }

        public void Dispose()
        {
            _provider.Dispose();
        }
    }
}
