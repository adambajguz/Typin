namespace TypinExamples.Infrastructure.WebWorkers.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.JSInterop;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Extensions;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Payloads;
    using TypinExamples.Infrastructure.WebWorkers.Common.Messaging;
    using TypinExamples.Infrastructure.WebWorkers.Common.Payloads;
    using TypinExamples.Infrastructure.WebWorkers.Core.Internal;
    using TypinExamples.Infrastructure.WebWorkers.Core.Internal.JS;
    using TypinExamples.Infrastructure.WebWorkers.WorkerCore;
    using TypinExamples.Infrastructure.WebWorkers.WorkerCore.Internal;

    public sealed class Worker<T> : IWorker
        where T : class, IWorkerStartup, new()
    {
        private readonly IdProvider _idProvider = new();
        private readonly string[] _assemblies;

        private readonly ISerializer _serializer;
        private readonly IJSRuntime _jsRuntime;
        private readonly ScriptLoader _scriptLoader;

        public ulong Id { get; }
        public bool IsDisposed { get; private set; }
        public bool IsInitialized { get; private set; }

        private readonly Dictionary<ulong, TaskCompletionSource<object>> messageRegister = new();

        public Worker(ulong id, IJSRuntime jsRuntime, string[] assemblies)
        {
            Id = id;

            _assemblies = assemblies;
            _jsRuntime = jsRuntime;
            _scriptLoader = new ScriptLoader(_jsRuntime);

            _serializer = new DefaultSerializer();
        }

        #region Messaging
        private async Task PostMessageAsync<TPayload>(Message<TPayload> message)
        {
            string? serialized = _serializer.Serialize(message);
            await _jsRuntime.InvokeVoidAsync($"{ScriptLoader.MODULE_NAME}.postMessage", Id, serialized);
        }

        [JSInvokable]
        public void OnMessage(string rawMessage)
        {
#if DEBUG
            Console.WriteLine($"{nameof(Worker<T>)}.{nameof(OnMessage)}: {rawMessage}");
#endif
            IMessage message = _serializer.Deserialize<IMessage>(rawMessage);

            if (message.Type.HasFlags(MessageTypes.Result))
            {
                if (!messageRegister.TryGetValue(message.Id, out var taskCompletionSource))
                    throw new InvalidOperationException($"Invalid message with call id {message.Id} from {message.WorkerId}.");

                taskCompletionSource.SetResult(message);
                messageRegister.Remove(message.Id);

                if (message.Exception is not null)
                    taskCompletionSource.SetException(message.Exception);
            }
        }

        public async Task NotifyAsync<TPayload>(TPayload payload)
        {
            var callId = _idProvider.Next();

            Message<TPayload> message = new()
            {
                Id = callId,
                WorkerId = Id,
                Type = MessageTypes.FromMain | MessageTypes.Call,
                Payload = payload
            };

            await PostMessageAsync(message);
        }

        public async Task CallCommandAsync<TPayload>(TPayload payload)
        {
            await CallCommandAsync<TPayload, CommandFinished>(payload);
        }

        public async Task<TResultPayload> CallCommandAsync<TPayload, TResultPayload>(TPayload payload)
        {
            var callId = _idProvider.Next();

            var taskCompletionSource = new TaskCompletionSource<object>();
            messageRegister.Add(callId, taskCompletionSource);

            Message<TPayload> message = new()
            {
                Id = callId,
                WorkerId = Id,
                Type = MessageTypes.FromMain | MessageTypes.Call,
                Payload = payload
            };

            await PostMessageAsync(message);

            if (await taskCompletionSource.Task is not Message<TResultPayload> returnMessage)
                throw new InvalidOperationException("Invalid message.");

            if (returnMessage.Exception is not null)
                throw new AggregateException($"Worker exception: {returnMessage.Exception.Message}", returnMessage.Exception);

            return returnMessage.Payload!;
        }
        #endregion

        public async Task InitAsync()
        {
            if (IsInitialized)
                return;

            await _scriptLoader.InitScript();

            var ms = typeof(MessageService);
            var wp = typeof(WorkerEntryPoint);

            var taskCompletionSource = new TaskCompletionSource<object>();
            messageRegister.Add(_idProvider.Next(), taskCompletionSource);

            await _jsRuntime.InvokeVoidAsync($"{ScriptLoader.MODULE_NAME}.initWorker",
                                             Id,
                                             DotNetObjectReference.Create(this),
                                             new WorkerInitOptions
                                             {
                                                 DependentAssemblyFilenames = _assemblies,
                                                 CallbackMethod = nameof(OnMessage),
                                                 MessageEndpoint = $"[{ms.Assembly.GetName().Name}]{ms.FullName}:{nameof(MessageService.OnMessage)}",
                                                 InitEndpoint = $"[{wp.Assembly.GetName().Name}]{wp.FullName}:{nameof(WorkerEntryPoint.Init)}",
                                                 StartupType = typeof(T).AssemblyQualifiedName,
                                                 Debug = false
                                             });

            if (await taskCompletionSource.Task is not Message<InitializedPayload> iwrm)
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

        public async ValueTask DisposeAsync()
        {
            if (IsDisposed)
                return;

            await CallCommandAsync(new DisposePayload());

            await _jsRuntime.InvokeVoidAsync($"{ScriptLoader.MODULE_NAME}.disposeWorker", Id);
            IsDisposed = true;
        }
    }
}
