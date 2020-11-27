namespace TypinExamples.Infrastructure.WebWorkers.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using Microsoft.JSInterop;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;
    using TypinExamples.Infrastructure.WebWorkers.BlazorBoot;
    using TypinExamples.Infrastructure.WebWorkers.Core.Internal;

    public sealed class WorkerFactory : IWorkerFactory
    {
        private readonly IdProvider _idProvider = new();
        private readonly Dictionary<ulong, IWorker> _workers = new();
        private string[]? _assemblies;

        private readonly IJSRuntime _jsRuntime;
        private readonly HttpClient _httpClient;

        public WorkerFactory(IJSRuntime jsRuntime, HttpClient httpClient)
        {
            _jsRuntime = jsRuntime;
            _httpClient = httpClient;
        }

        public async Task<IWorker> CreateAsync<T>()
            where T : class, IWorkerStartup, new()
        {
            _assemblies ??= await GetAssembliesToLoad() ?? throw new ApplicationException("Failed to fetch assemblies list.");

            Worker<T> worker = new Worker<T>(_idProvider.Next(), _jsRuntime, _assemblies);
            await worker.InitAsync();
            _workers.Add(worker.Id, worker);

            return worker;
        }

        public IWorker? GetWorkerOrDefault(ulong id)
        {
            _workers.TryGetValue(id, out IWorker? value);
            return value;
        }

        private async Task<string[]?> GetAssembliesToLoad()
        {
            BlazorBootModel? response = await _httpClient.GetFromJsonAsync<BlazorBootModel>(BlazorBootModel.FilePath);
            string[]? assemblies = response?.Resources.Assembly.Keys.ToArray();

            //if (assemblies is not null)
            //{
            //    Dictionary<string, byte[]> dlls = new();
            //    foreach (var assembly in assemblies)
            //    {
            //        var dll = await _httpClient.GetByteArrayAsync($"_framework/{assembly}");
            //        dlls.Add(assembly, dll);
            //    }

            //    _dlls = dlls;
            //}

            return assemblies;
        }

        public async ValueTask DisposeAsync()
        {
            foreach (var worker in _workers)
                await worker.Value.DisposeAsync();
        }
    }
}
