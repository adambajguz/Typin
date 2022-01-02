namespace TypinExamples.Infrastructure.WebWorkers.Core
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.JSInterop;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;
    using TypinExamples.Infrastructure.WebWorkers.BlazorBoot;
    using TypinExamples.Infrastructure.WebWorkers.Core.Internal;
    using TypinExamples.Infrastructure.WebWorkers.Core.Internal.JS;

    public sealed class WorkerFactory : IWorkerFactory
    {
        private readonly IdProvider _idProvider = new();
        private string[]? _assemblies;

        private readonly IJSRuntime _jsRuntime;
        private readonly HttpClient _httpClient;
        private readonly IWorkerManager _workerManager;
        private readonly IMessagingService _messagingService;
        private readonly IMessagingProvider _messagingProvider;
        private readonly IScriptLoader _scriptLoader;
        private readonly ILogger _logger;

        public WorkerFactory(IJSRuntime jsRuntime,
                             HttpClient httpClient,
                             IWorkerManager workerManager,
                             IMessagingService messagingService,
                             IMessagingProvider messagingProvider,
                             IScriptLoader scriptLoader,
                             ILogger<WorkerFactory> logger)
        {
            _jsRuntime = jsRuntime;
            _httpClient = httpClient;
            _workerManager = workerManager;
            _messagingService = messagingService;
            _messagingProvider = messagingProvider;
            _scriptLoader = scriptLoader;
            _logger = logger;
        }

        public async Task<IWorker> CreateAsync<T>(Action<WorkerCreationConfiguration>? creationConfiguration = null,
                                                  Action<ulong>? onInitStarted = null,
                                                  Action<ulong>? onWorkerCreated = null)
            where T : class, IWorkerStartup, new()
        {
            WorkerCreationConfiguration workerCreationConfiguration = new();
            creationConfiguration?.Invoke(workerCreationConfiguration);

            workerCreationConfiguration.ExcludedAssemblied ??= Array.Empty<string>(); //just in case user set it to null

            if (!(workerCreationConfiguration.IncludedAssemblies?.Any() ?? false))
            {
                _assemblies ??= await GetAssembliesToLoad() ?? throw new ApplicationException("Failed to fetch assemblies list.");
                workerCreationConfiguration.IncludedAssemblies = _assemblies;
            }

            ulong workerId = _idProvider.Next();
            Worker<T> worker = new Worker<T>(workerId,
                                             () => DisposeCallback(workerId),
                                             _jsRuntime,
                                             _messagingService,
                                             _messagingProvider,
                                             workerCreationConfiguration,
                                             _scriptLoader,
                                             _logger);

            onInitStarted?.Invoke(workerId);

            _logger.LogInformation("Initializing worker ({Id})", worker.Id);
            await worker.InitAsync();
            _workerManager.AddWorker(worker);

            _logger.LogInformation("Created worker ({Id})", worker.Id);
            onWorkerCreated?.Invoke(workerId);

            return worker;
        }

        #region Helpers
        private void DisposeCallback(ulong id)
        {
            if (_workerManager.RemoveWorker(id))
            {
                _logger.LogInformation("Removed worker {Id}", id);
            }
        }

        private async Task<string[]?> GetAssembliesToLoad()
        {
            _logger.LogInformation("Fetching assemblies list.");

            BlazorBootModel? response = await _httpClient.GetFromJsonAsync<BlazorBootModel>(BlazorBootModel.FilePath);
            string[]? assemblies = response?.Resources.Assembly.Keys.ToArray();

            _logger.LogInformation("Fetched assemblies list.");

            //if (assemblies is not null)
            //{
            //    Dictionary<string, byte[]> dlls = new();
            //    foreach (var assembly in assemblies)
            //    {
            //        var dll = await _httpClient.GetByteArrayAsync($"_framework/{assembly}");
            //        dlls.Add(assembly, dll);
            //    }
            //
            //    var serialized = JsonConvert.SerializeObject(dlls);
            //    int count = serialized.Length;
            //}

            return assemblies;
        }
        #endregion
    }
}
