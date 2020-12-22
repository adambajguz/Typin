namespace TypinExamples.Infrastructure.WebWorkers.Core
{
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
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
        private readonly WorkerCreationConfiguration _workerCreationConfiguration;

        private readonly ISerializer _serializer;
        private readonly IJSRuntime _jsRuntime;
        private readonly IMessagingService _messagingService;
        private readonly IMessagingProvider _messagingProvider;
        private readonly ScriptLoader _scriptLoader;
        private readonly ILogger _logger;

        public ulong Id { get; }
        public bool IsCancelled { get; private set; }
        public bool IsDisposed { get; private set; }
        public bool IsInitialized { get; private set; }

        public Worker(ulong id,
                      Action disposeCallback,
                      IJSRuntime jsRuntime,
                      IMessagingService messagingService,
                      IMessagingProvider messagingProvider,
                      WorkerCreationConfiguration workerCreationConfiguration,
                      ILogger logger)
        {
            Id = id;

            _disposeCallback = disposeCallback;
            _workerCreationConfiguration = workerCreationConfiguration;
            _jsRuntime = jsRuntime;
            _messagingService = messagingService;
            _messagingProvider = messagingProvider;

            _scriptLoader = new ScriptLoader(_jsRuntime);
            _serializer = new DefaultSerializer();

            _logger = logger;
        }

        public async Task NotifyAsync<TPayload>(TPayload payload)
            where TPayload : INotification
        {
            await _messagingService.NotifyAsync(Id, payload);
        }

        public async Task CallCommandAsync<TPayload>(TPayload payload)
            where TPayload : ICommand
        {
            await _messagingService.CallCommandAsync<TPayload, CommandFinished>(Id, payload);
        }

        public async Task<TResultPayload> CallCommandAsync<TPayload, TResultPayload>(TPayload payload)
            where TPayload : ICommand<TResultPayload>
        {
            return await _messagingService.CallCommandAsync<TPayload, TResultPayload>(Id, payload);
        }

        public async Task InitAsync()
        {
            if (IsInitialized)
                return;

            await _scriptLoader.InitScript();

            Type ms = typeof(WorkerThreadMessagingProvider);
            Type wp = typeof(WorkerEntryPoint);

            (ulong callId, Task<object> task) = _messagingService.ReserveId(Id);

            string[] dependentAssemblyFilenames = _workerCreationConfiguration.IncludedAssemblies.Except(_workerCreationConfiguration.ExcludedAssemblied).ToArray();

            await _jsRuntime.InvokeVoidAsync($"{ScriptLoader.MODULE_NAME}.initWorker",
                                             Id,
                                             DotNetObjectReference.Create((MainThreadMessagingProvider)_messagingProvider),
                                             new WorkerInitOptions
                                             {
                                                 DependentAssemblyFilenames = dependentAssemblyFilenames,
                                                 CallbackMethod = nameof(MainThreadMessagingProvider.OnMessage),
                                                 MessageEndpoint = $"[{ms.Assembly.GetName().Name}]{ms.FullName}:{nameof(WorkerThreadMessagingProvider.InternalOnMessage)}",
                                                 InitEndpoint = $"[{wp.Assembly.GetName().Name}]{wp.FullName}:{nameof(WorkerEntryPoint.Init)}",
                                                 InitCallId = callId,
                                                 StartupType = typeof(T).AssemblyQualifiedName,
                                                 Debug = false
                                             });

            if (await task is not Message<InitializeResult> iwrm)
            {
                throw new InvalidOperationException($"Failed to init worker with id {Id}.");
            }

            IsInitialized = true;
        }

        public async Task<int> RunAsync()
        {
            RunProgramResult result = await CallCommandAsync<RunProgramCommand, RunProgramResult>(new RunProgramCommand
            {
                ProgramClass = typeof(T).AssemblyQualifiedName ?? throw new ApplicationException($"{typeof(T).Name} is a generic type.")
            });

            return result.ExitCode;
        }

        public async Task CancelAsync()
        {
            await CallCommandAsync(new CancelCommand());
            IsCancelled = true;
        }

        public async Task CancelAsync(TimeSpan delay)
        {
            await CallCommandAsync(new CancelCommand { Delay = delay });
        }

        public async ValueTask DisposeAsync()
        {
            if (IsDisposed)
                return;

            const int miliseconds = 3000;
            TaskAwaiter awaiter = DisposeWorkerAsync().GetAwaiter();
            await Task.Delay(miliseconds);

            bool forcedToDispose = false;
            if (!awaiter.IsCompleted)
            {
                _logger.LogWarning("Worker {Id} does not respond, thus failed to dispose within {Time} miliseconds.", Id, miliseconds);
                forcedToDispose = true;
            }

            IsDisposed = true;

            await _jsRuntime.InvokeVoidAsync($"{ScriptLoader.MODULE_NAME}.disposeWorker", Id);

            _messagingService.CleanMessageRegistry(Id);

            if (forcedToDispose)
                _logger.LogInformation("Worker {Id} was forced to dispose.", Id);
            else
                _logger.LogInformation("Worker {Id} was disposed.", Id);

            _disposeCallback();
        }

        private async Task DisposeWorkerAsync()
        {
            await CallCommandAsync(new DisposeCommand());
            IsDisposed = true;
        }
    }
}
