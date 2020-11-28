namespace TypinExamples.Infrastructure.WebWorkers.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.JSInterop;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;
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

        #region Messages
        [JSInvokable]
        public void OnMessage(string rawMessage)
        {
#if DEBUG
            Console.WriteLine($"{nameof(Worker<T>)}.{nameof(OnMessage)}: {rawMessage}");
#endif
            IMessage message = _serializer.Deserialize<IMessage>(rawMessage);

            if (!messageRegister.TryGetValue(message.Id, out var taskCompletionSource))
                throw new InvalidOperationException($"Invalid message with call id {message.Id} from {message.WorkerId}.");

            taskCompletionSource.SetResult(message);
            messageRegister.Remove(message.Id);

            if (message.Exception is not null)
                taskCompletionSource.SetException(message.Exception);
        }

        public async Task PostMessageAsync<TMessage>(TMessage message)
            where TMessage : IMessage
        {
            string? serialized = _serializer.Serialize(message);

#if DEBUG
            Console.WriteLine($"{nameof(Worker<T>)}.{nameof(PostMessageAsync)}: {serialized}");
#endif

            await _jsRuntime.InvokeVoidAsync($"{ScriptLoader.MODULE_NAME}.postMessage", Id, serialized);
        }

        private async Task<TResultMessage> PostMessageAsync<TMessage, TResultMessage>(Func<WorkerCallContext, TMessage> action)
            where TMessage : class, IMessage
            where TResultMessage : notnull, IMessage
        {
            var callId = _idProvider.Next();

            var taskCompletionSource = new TaskCompletionSource<object>();
            messageRegister.Add(callId, taskCompletionSource);

            IMessage message = action.Invoke(new WorkerCallContext { WorkerId = Id, CallId = callId });
            await PostMessageAsync(message);

            if (await taskCompletionSource.Task is not TResultMessage returnMessage)
                throw new InvalidOperationException("Invalid message.");

            if (returnMessage.Exception is not null)
                throw new AggregateException($"Worker exception: {returnMessage.Exception.Message}", returnMessage.Exception);

            return returnMessage;
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

            if (await taskCompletionSource.Task is not Message<Init.ResultPayload> iwrm)
            {
                throw new InvalidOperationException($"Failed to init worker with id {Id}.");
            }

            IsInitialized = true;
        }

        public async Task<int> RunAsync()
        {
            var result = await PostMessageAsync<RunProgramMessage, RunProgramResultMessage>((context) =>
            {
                return new RunProgramMessage
                {
                    WorkerId = context.WorkerId,
                    CallId = context.CallId,
                    ProgramClass = typeof(T).AssemblyQualifiedName ?? throw new ApplicationException($"{typeof(T).Name} is a generic type.")
                };
            });

            if (result.Exception is not null)
                throw new AggregateException($"Worker exception: {result.Exception.Message}", result.Exception);

            return result.ExitCode;
        }

        public async Task CancelAsync()
        {
            var result = await PostMessageAsync<CancelMessage, CancelResultMessage>((context) =>
            {
                return new CancelMessage
                {
                    WorkerId = context.WorkerId,
                    CallId = context.CallId
                };
            });

            if (result.Exception is not null)
                throw new AggregateException($"Worker exception: {result.Exception.Message}", result.Exception);
        }

        public async Task<TResponse> CallAsync<TRequest, TResponse>(TRequest data)
        {
            var result = await PostMessageAsync<CallPayload<TRequest>, CallResultMessage<TResponse>>((context) =>
            {
                return new CallPayload<TRequest>
                {
                    WorkerId = context.WorkerId,
                    CallId = context.CallId,
                    ProgramClass = typeof(T).AssemblyQualifiedName ?? throw new ApplicationException($"{typeof(T).Name} is a generic type."),
                    Payload = data
                };
            });

            if (result.Exception is not null)
                throw new AggregateException($"Worker exception: {result.Exception.Message}", result.Exception);

            return result.Payload;
        }

        public async ValueTask DisposeAsync()
        {
            if (IsDisposed)
                return;

            var result = await PostMessageAsync<DisposeInstanceMessage, DisposeInstanceResultMessage>((context) =>
            {
                return new DisposeInstanceMessage
                {
                    WorkerId = context.WorkerId,
                    CallId = context.CallId
                };
            });

            await _jsRuntime.InvokeVoidAsync($"{ScriptLoader.MODULE_NAME}.disposeWorker", Id);
            IsDisposed = true;
        }
    }
}
