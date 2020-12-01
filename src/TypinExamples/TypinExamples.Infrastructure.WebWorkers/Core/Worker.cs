namespace TypinExamples.Infrastructure.WebWorkers.Core
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.JSInterop;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Payloads;
    using TypinExamples.Infrastructure.WebWorkers.Common.Messaging;
    using TypinExamples.Infrastructure.WebWorkers.Common.Payloads;
    using TypinExamples.Infrastructure.WebWorkers.Core.Internal;
    using TypinExamples.Infrastructure.WebWorkers.Core.Internal.JS;
    using TypinExamples.Infrastructure.WebWorkers.Core.Internal.Messaging;
    using TypinExamples.Infrastructure.WebWorkers.WorkerCore.Internal;
    using TypinExamples.Infrastructure.WebWorkers.WorkerCore.Internal.Messaging;

    public sealed class Worker<T> : IWorker
        where T : class, IWorkerStartup, new()
    {
        private readonly Action _disposeCallback;
        private readonly string[] _assemblies;

        private readonly ISerializer _serializer;
        private readonly IJSRuntime _jsRuntime;
        private readonly IMessagingService _messagingService;
        private readonly IMessagingProvider _messagingProvider;
        private readonly ScriptLoader _scriptLoader;

        public ulong Id { get; }
        public bool IsDisposed { get; private set; }
        public bool IsInitialized { get; private set; }

        public Worker(ulong id,
                      Action disposeCallback,
                      IJSRuntime jsRuntime,
                      IMessagingService messagingService,
                      IMessagingProvider messagingProvider,
                      string[] assemblies)
        {
            Id = id;

            _disposeCallback = disposeCallback;
            _assemblies = assemblies;
            _jsRuntime = jsRuntime;
            _messagingService = messagingService;
            _messagingProvider = messagingProvider;

            _scriptLoader = new ScriptLoader(_jsRuntime);
            _serializer = new DefaultSerializer();
        }

        public async Task NotifyAsync<TPayload>(TPayload payload)
        {
            await _messagingService.NotifyAsync(Id, payload);
        }

        public async Task CallCommandAsync<TPayload>(TPayload payload)
        {
            await _messagingService.CallCommandAsync<TPayload, CommandFinished>(Id, payload);
        }

        public async Task<TResultPayload> CallCommandAsync<TPayload, TResultPayload>(TPayload payload)
        {
            return await _messagingService.CallCommandAsync<TPayload, TResultPayload>(Id, payload);
        }

        public async Task InitAsync()
        {
            if (IsInitialized)
                return;

            await _scriptLoader.InitScript();

            var ms = typeof(WorkerThreadMessagingProvider);
            var wp = typeof(WorkerEntryPoint);

            (ulong callId, Task<object> task) = _messagingService.ReserveId();

            await _jsRuntime.InvokeVoidAsync($"{ScriptLoader.MODULE_NAME}.initWorker",
                                             Id,
                                             DotNetObjectReference.Create((MainThreadMessagingProvider)_messagingProvider),
                                             new WorkerInitOptions
                                             {
                                                 DependentAssemblyFilenames = _assemblies,
                                                 CallbackMethod = nameof(MainThreadMessagingProvider.OnMessage),
                                                 MessageEndpoint = $"[{ms.Assembly.GetName().Name}]{ms.FullName}:{nameof(WorkerThreadMessagingProvider.InternalOnMessage)}",
                                                 InitEndpoint = $"[{wp.Assembly.GetName().Name}]{wp.FullName}:{nameof(WorkerEntryPoint.Init)}",
                                                 InitCallId = callId,
                                                 StartupType = typeof(T).AssemblyQualifiedName,
                                                 Debug = false
                                             });

            if (await task is not Message<InitializedPayload> iwrm)
            {
                throw new InvalidOperationException($"Failed to init worker with id {Id}.");
            }

            IsInitialized = true;
        }

        public async Task<int> RunAsync()
        {
            ProgramFinishedPayload result = await CallCommandAsync<RunProgramPayload, ProgramFinishedPayload>(new RunProgramPayload
            {
                ProgramClass = typeof(T).AssemblyQualifiedName ?? throw new ApplicationException($"{typeof(T).Name} is a generic type.")
            });

            return result.ExitCode;
        }

        public async Task CancelAsync()
        {
            await CallCommandAsync(new CancelPayload());
        }

        public async Task CancelAsync(TimeSpan delay)
        {
            await CallCommandAsync(new CancelPayload { Delay = delay });
        }

        public async ValueTask DisposeAsync()
        {
            if (IsDisposed)
                return;

            await CallCommandAsync(new DisposePayload());

            await _jsRuntime.InvokeVoidAsync($"{ScriptLoader.MODULE_NAME}.disposeWorker", Id);
            IsDisposed = true;

            _disposeCallback();
        }
    }
}
