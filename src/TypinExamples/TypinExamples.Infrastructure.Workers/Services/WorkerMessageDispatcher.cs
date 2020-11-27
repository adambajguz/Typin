namespace TypinExamples.Infrastructure.Workers.Services
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
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using TypinExamples.Application.Services;
    using TypinExamples.Application.Services.Workers;
    using TypinExamples.Domain.Interfaces;
    using TypinExamples.Domain.Models.BlazorBoot;
    using TypinExamples.Domain.Models.Workers;
    using TypinExamples.Infrastructure.Workers.Configuration;
    using TypinExamples.Infrastructure.Workers.Models;
    using TypinExamples.Infrastructure.Workers.WorkerServices;

    public class WorkerMessageDispatcher : IWorkerMessageDispatcher
    {
        private string[]? _assemblies;
        private readonly List<WorkerDescriptor> _workers = new();

        private readonly ILogger _logger;
        private readonly IWorkerFactory _workerFactory;
        private readonly HttpClient _client;
        private readonly IMediator _mediator;
        private readonly WorkersSettings _options;
        private readonly TimerService _timer;

        public WorkerMessageDispatcher(ILogger<WorkerMessageDispatcher> logger,
                                       IWorkerFactory workerFactory,
                                       HttpClient client,
                                       IMediator mediator,
                                       IOptions<WorkersSettings> options,
                                       TimerService timer)
        {
            _logger = logger;
            _workerFactory = workerFactory;
            _client = client;
            _mediator = mediator;
            _options = options.Value;
            _timer = timer;
        }

        public async Task<WorkerResult> DispachAsync(WorkerMessage model)
        {
            _logger.LogInformation("DispachAsync '{Id}' '{TargetType}'.", model.Id, model.TargetType);

            WorkerDescriptor? descriptor = model.WorkerId is long wid ? GetWorkerOrDefault(wid) : await GetWorkerDescriptor();

            if (descriptor is null)
                descriptor = await GetWorkerDescriptor();

            model.WorkerId = descriptor.Worker.Identifier;
            string serializedModel = JsonConvert.SerializeObject(model);

            if (model.IsNotification)
            {
                await descriptor.Worker.PostMessageAsync(serializedModel);
                descriptor.IsBusy = false;

                return new WorkerResult
                {
                    MessageId = model.Id,
                    WorkerId = model.WorkerId,
                    FromWorker = true
                };
            }
            else
            {
                string result = await descriptor.BackgroundService.RunAsync(s => s.Dispatch(serializedModel));
                descriptor.IsBusy = false;

                WorkerResult workerMessage = JsonConvert.DeserializeObject<WorkerResult>(result);

                return workerMessage;
            }
        }

        private async void OnIncomingMessageFromWorkerToMain(object? sender, string e)
        {
            if (string.IsNullOrWhiteSpace(e))
                return;

            WorkerMessage model = JsonConvert.DeserializeObject<WorkerMessage>(e);

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
                        await _mediator.Publish(obj);
                    }
                    else
                    {
                        object? x = await _mediator.Send(obj);
                    }
                }
            }
        }

        #region Helpers
        private WorkerDescriptor? GetWorkerOrDefault(long id)
        {
            return _workers.Where(x => x.Worker.Identifier == id)
                           .FirstOrDefault();
        }

        private async Task<WorkerDescriptor> GetWorkerDescriptor()
        {
            WorkerDescriptor? descriptor = _workers.OrderByDescending(x => x.WGCLifetime)
                                                   .FirstOrDefault(x => x.IsReady);

            if (descriptor is null)
            {
                _assemblies ??= await GetAssembliesToLoad();

                // Create worker.
                IWorker worker = await _workerFactory.CreateAsync();

                // Create service reference. For most scenarios, it's safe (and best) to keep this reference around somewhere to avoid the startup cost.
                IWorkerBackgroundService<WorkerService> service = await worker.CreateBackgroundServiceAsync<WorkerService>((cfg) =>
                {
                    cfg.AddAssemblies(_assemblies);
                });

                worker.IncomingMessage += OnIncomingMessageFromWorkerToMain;

                descriptor = new WorkerDescriptor
                {
                    Worker = worker,
                    BackgroundService = service,
                    IsBusy = true,
                };
                _workers.Add(descriptor);

                if (!_timer.IsRunning)
                {
                    _timer.Elapsed += WorkersGarbageCollector;
                    _timer.Set(_options.WGCInterval < 100 ? 100 : _options.WGCInterval, true);
                }

                _logger.LogInformation("Created a new worker '{WorkerDescriptor}'.", descriptor);
            }

            descriptor.WGCLifetime = GetLifetime();
            descriptor.IsBusy = true;
            _logger.LogInformation("Running task on worker '{WorkerDescriptor}'.", descriptor);

            return descriptor;
        }

        private int GetLifetime()
        {
            const int FALLBACK = 8;

            int count = _workers.Count;
            int optionsCount = (_options?.WorkerWGCLifetime?.Length ?? 0);

            if (optionsCount == 0)
                return FALLBACK;

            int tmp;
            if (optionsCount > count)
                tmp = _options!.WorkerWGCLifetime![count];
            else
                tmp = _options!.WorkerWGCLifetime![0];

            return tmp < 1 ? FALLBACK : tmp;
        }

        private async Task<string[]?> GetAssembliesToLoad()
        {
            BlazorBootModel? response = await _client.GetFromJsonAsync<BlazorBootModel>(BlazorBootModel.FilePath);
            string[]? assemblies = response?.Resources.Assembly.Keys.ToArray();

            _logger.LogInformation("Fetched assemblies list.");

            return assemblies;
        }
        #endregion

        public async ValueTask DisposeAsync()
        {
            foreach (WorkerDescriptor worker in _workers)
                await worker.Worker.DisposeAsync();

            _workers.Clear();
        }

        private async void WorkersGarbageCollector()
        {
            int count = _workers.Count;
            if (count == 0)
            {
                _timer.Stop();
                return;
            }

            IOrderedEnumerable<WorkerDescriptor> workers = _workers.Where(x => !x.IsBusy)
                                                                   .OrderBy(x => x.WGCLifetime);

            foreach (WorkerDescriptor worker in workers)
            {
                if (--worker.WGCLifetime < -1)
                    await worker.Worker.DisposeAsync();
            }

            _workers.RemoveAll(x => x.WGCLifetime < -1);

            if (count != _workers.Count)
                _logger.LogInformation("WGC sweap '{Before}' -> '{After}'.", count, _workers.Count);
        }
    }
}
