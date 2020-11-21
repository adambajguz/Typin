namespace TypinExamples.Workers.Services
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using BlazorWorker.WorkerCore;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using TypinExamples.Core;
    using TypinExamples.Core.Services;
    using TypinExamples.Domain.Extensions;
    using TypinExamples.Domain.Interfaces;
    using TypinExamples.Domain.Models;
    using TypinExamples.Infrastructure.Workers.Behaviors;

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

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TracingBehavior<,>))
                    .AddSingleton<ICoreTaskDispatcher, CoreTaskDispatcher>()
                    .AddSingleton<HttpClient>(_httpClient)
                    .AddSingleton<IWorkerMessageService>(_messageService);

            return services.BuildServiceProvider();
        }

        private async void OnIncomingMessageFromMainToWorker(object? sender, string e)
        {
            WorkerMessageModel model = JsonConvert.DeserializeObject<WorkerMessageModel>(e);
            IMediator mediator = _provider.GetRequiredService<IMediator>();

            await Dispatch(model, mediator);
        }

        public async Task<string> Execute(string data)
        {
            Console.WriteLine(data);
            WorkerMessageModel deserialized = JsonConvert.DeserializeObject<WorkerMessageModel>(data);

            WorkerMessageModel result = await Execute(deserialized);

            return JsonConvert.SerializeObject(result);
        }

        public async Task<WorkerMessageModel> Execute(WorkerMessageModel model)
        {
            IMediator mediator = _provider.GetRequiredService<IMediator>();

            return await Dispatch(model, mediator);
        }

        private async Task<WorkerMessageModel> Dispatch(WorkerMessageModel model, IMediator mediator)
        {
            try
            {
                if (model.TargetType is string targetType &&
                    model.Data is string data)
                {
                    Type? type = Type.GetType(targetType);

                    if (type is Type t && JsonConvert.DeserializeObject(data, t) is object obj)
                    {
                        if (obj is IWorkerIdentifiable wi)
                        {
                            wi.WorkerId = model.WorkerId;
                        }

                        if (model.IsNotification)
                        {
                            await mediator.Publish(obj);
                        }
                        else
                        {
                            object? x = await mediator.Send(obj);

                            return x as WorkerMessageModel ?? WorkerMessageModel.Empty;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return this.CreateMessageBuilder()
                           .HandleException(model.WorkerId ?? -1, ex)
                           .Build();
            }

            return WorkerMessageModel.Empty;
        }

        public void Dispose()
        {
            _provider.Dispose();
        }
    }
}
