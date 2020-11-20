namespace TypinExamples.Workers.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using BlazorWorker.BackgroundServiceFactory;
    using BlazorWorker.Core;
    using BlazorWorker.WorkerBackgroundService;
    using MediatR;
    using Newtonsoft.Json;
    using TypinExamples.Common.Models;
    using TypinExamples.Core.Services;
    using TypinExamples.Domain.Models;
    using TypinExamples.Workers.Models;

    public class WorkerTaskDispatcher : IWorkerTaskDispatcher
    {
        private readonly List<WorkerDescriptor> _workers = new();
        private readonly Dictionary<Guid, WorkerDescriptor> _tasksLookup = new();

        private readonly IMediator _mediator;
        private readonly IWorkerFactory _workerFactory;
        private readonly HttpClient _client;
        private string[]? _assemblies;

        public WorkerTaskDispatcher(IWorkerFactory workerFactory,
                                    HttpClient client,
                                    IMediator mediator)
        {
            _workerFactory = workerFactory;
            _client = client;
            _mediator = mediator;
        }

        public async Task<WorkerMessageModel> RunAsync(WorkerMessageModel model)
        {
            WorkerDescriptor descriptor = await GetWorkerDescriptor();
            descriptor.IsInUse = true;

            _tasksLookup.Add(model.Id, descriptor);

            string serializedModel = JsonConvert.SerializeObject(model);

            string result = await descriptor.BackgroundService.RunAsync(s => s.Execute(serializedModel));
            _tasksLookup.Remove(model.Id);

            return JsonConvert.DeserializeObject<WorkerMessageModel>(result);
        }

        public WorkerDescriptor? GetWorkerOrDefault(Guid id)
        {
            _tasksLookup.TryGetValue(id, out WorkerDescriptor? workerDescriptor);

            return workerDescriptor;
        }

        public async Task<bool> SendMessageAsync(Guid id, WorkerMessageModel model)
        {
            WorkerDescriptor? descriptor = GetWorkerOrDefault(id);

            if (descriptor is null)
                return false;

            string serializedModel = JsonConvert.SerializeObject(model);

            await descriptor.Worker.PostMessageAsync(serializedModel);

            return true;
        }

        private async Task<WorkerDescriptor> GetWorkerDescriptor()
        {
            WorkerDescriptor? worker = _workers.FirstOrDefault(x => !x.IsInUse);

            if (worker is null)
            {
                _assemblies ??= await GetAssembliesToLoad();

                // Create worker.
                IWorker w = await _workerFactory.CreateAsync();

                // Create service reference. For most scenarios, it's safe (and best) to keep this reference around somewhere to avoid the startup cost.
                IWorkerBackgroundService<WorkerService> service = await w.CreateBackgroundServiceAsync<WorkerService>((cfg) =>
                {
                    cfg.AddAssemblies(_assemblies);
                });

                w.IncomingMessage += OnIncomingMessageFromWorkerToMain;

                worker = new WorkerDescriptor
                {
                    Worker = w,
                    BackgroundService = service,
                    IsInUse = true
                };
                _workers.Add(worker);
            }

            return worker;
        }

        private void OnIncomingMessageFromWorkerToMain(object? sender, string e)
        {
            WorkerMessageModel model = JsonConvert.DeserializeObject<WorkerMessageModel>(e);

        }

        private async Task<string[]?> GetAssembliesToLoad()
        {
            BlazorBoot? response = await _client.GetFromJsonAsync<BlazorBoot>("_framework/blazor.boot.json");
            string[]? assemblies = response?.Resources.Assembly.Keys.ToArray();

            return assemblies;
        }

        public async ValueTask DisposeAsync()
        {
            foreach (var worker in _workers)
            {
                await worker.BackgroundService.DisposeAsync();
            }
            _workers.Clear();
        }
    }
}
