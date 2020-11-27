namespace TypinExamples.Infrastructure.WebWorkers.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.JSInterop;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;
    using TypinExamples.Infrastructure.WebWorkers.Core.Internal;
    using TypinExamples.Infrastructure.WebWorkers.Hil;
    using TypinExamples.Infrastructure.WebWorkers.Hil.Messages.Base;
    using TypinExamples.Infrastructure.WebWorkers.WorkerCore;

    public sealed class Worker<T> : IWorker
        where T : class, IWorkerStartup, new()
    {
        private readonly Dictionary<Type, Action<BaseMessage>> _messageHandlerRegistry = new();
        private readonly string[] _assemblies;

        private readonly ISerializer _serializer;
        private readonly IJSRuntime _jsRuntime;
        private readonly ScriptLoader _scriptLoader;

        public ulong Id { get; }
        public bool IsDisposed { get; private set; }
        public bool IsInitialized { get; private set; }

        private static ulong messageRegisterIdSource;
        private readonly Dictionary<ulong, TaskCompletionSource<MethodCallResultMessage>> messageRegister = new();

        public Worker(ulong id, IJSRuntime jsRuntime, string[] assemblies)
        {
            Id = id;

            _assemblies = assemblies;
            _jsRuntime = jsRuntime;
            _scriptLoader = new ScriptLoader(_jsRuntime);

            _serializer = new DefaultSerializer();

            _messageHandlerRegistry.Add(typeof(InitInstanceCompleteMessage), OnInitInstanceComplete);
            _messageHandlerRegistry.Add(typeof(InitWorkerCompleteMessage), OnInitWorkerComplete);
            _messageHandlerRegistry.Add(typeof(DisposeInstanceCompleteMessage), OnDisposeInstanceComplete);
            _messageHandlerRegistry.Add(typeof(MethodCallResultMessage), OnMethodCallResult);
        }

        public async ValueTask DisposeAsync()
        {
            if (disposeTask != null)
                await disposeTask.Task;

            if (IsDisposed)
                return;

            disposeTask = new TaskCompletionSource<bool>();

            await PostMessageAsync(new DisposeInstanceMessage
            {
                CallId = ++messageRegisterIdSource
            });

            await _jsRuntime.InvokeVoidAsync($"{ScriptLoader.ModuleName}.disposeWorker", Id);

            await disposeTask.Task;
        }

        public async Task<int> RunAsync()
        {
            await InitAsync();

            int exitCode = await InvokeAsyncInternal();

            return exitCode;
        }

        private async Task<int> InvokeAsyncInternal()
        {
            var id = ++messageRegisterIdSource;
            // If Blazor ever gets multithreaded this would need to be locked for race conditions
            // However, when/if that happens, most of this project is obsolete anyway
            var taskCompletionSource = new TaskCompletionSource<MethodCallResultMessage>();
            messageRegister.Add(id, taskCompletionSource);

            await PostMessageAsync(new MethodCallParamsMessage
            {
                WorkerId = Id,
                ProgramClass = typeof(T).AssemblyQualifiedName ?? throw new ApplicationException($"{typeof(T).Name} is a generic type."),
                CallId = id
            });

            var returnMessage = await taskCompletionSource.Task;

            if (returnMessage.Exception is not null)
                throw new AggregateException($"Worker exception: {returnMessage.Exception.Message}", returnMessage.Exception);

            return returnMessage.ExitCode;
        }

        [JSInvokable]
        public async Task OnMessage(string rawMessage)
        {
#if DEBUG
            Console.WriteLine($"{nameof(Worker<T>)}.{nameof(OnMessage)}: {rawMessage}");
#endif
            BaseMessage message = _serializer.Deserialize<BaseMessage>(rawMessage);

            if (_messageHandlerRegistry.TryGetValue(message.GetType(), out var value))
            {
                value.Invoke(message);
            }
        }

        public async Task PostMessageAsync<TMessage>(TMessage message)
            where TMessage : BaseMessage
        {
            string? serialized = _serializer.Serialize(message);

#if DEBUG
            Console.WriteLine($"{nameof(Worker<T>)}.{nameof(PostMessageAsync)}: {serialized}");
#endif

            await _jsRuntime.InvokeVoidAsync($"{ScriptLoader.ModuleName}.postMessage", Id, serialized);
        }

        #region core events
        private TaskCompletionSource<bool> initTask;
        private TaskCompletionSource<bool> disposeTask;
        private TaskCompletionSource<bool> initWorkerTask;

        private void OnDisposeInstanceComplete(BaseMessage message)
        {
            if (message is DisposeInstanceCompleteMessage m)
            {
                if (m.IsSuccess)
                {
                    disposeTask.SetResult(true);
                    IsDisposed = true;
                }
                else
                    disposeTask.SetException(m.Exception);
            }
        }

        public async Task InitAsync()
        {
            if (initTask != null)
                await initTask.Task;

            if (IsInitialized)
                return;

            initTask = new TaskCompletionSource<bool>();
            initWorkerTask = new TaskCompletionSource<bool>();

            {
                await _scriptLoader.InitScript();

                var wim = typeof(WorkerInstanceManager);
                var ms = typeof(MessageService);

                await _jsRuntime.InvokeVoidAsync(
                    $"{ScriptLoader.ModuleName}.initWorker",
                    Id,
                    DotNetObjectReference.Create(this),
                    new WorkerInitOptions
                    {
                        DependentAssemblyFilenames = _assemblies,
                        CallbackMethod = nameof(OnMessage),
                        MessageEndPoint = $"[{ms.Assembly.GetName().Name}]{ms.FullName}:{nameof(MessageService.OnMessage)}",
                        InitEndPoint = $"[{wim.Assembly.GetName().Name}]{wim.FullName}:{nameof(WorkerInstanceManager.Init)}"
                    });
            }

            await initWorkerTask.Task;

            await PostMessageAsync(new InitInstanceMessage()
            {
                WorkerId = Id, // TODO: This should not really be necessary?
                StartupType = typeof(T).AssemblyQualifiedName,
                CallId = ++messageRegisterIdSource
            });

            await initTask.Task;
        }

        private void OnMethodCallResult(BaseMessage message)
        {
            if (message is MethodCallResultMessage m)
            {
                if (!messageRegister.TryGetValue(m.CallId, out var taskCompletionSource))
                    return;

                taskCompletionSource.SetResult(m);
                messageRegister.Remove(m.CallId);
            }
        }

        private void OnInitWorkerComplete(BaseMessage message)
        {
            if (message is InitWorkerCompleteMessage m)
            {
                initWorkerTask.SetResult(true);
            }
        }

        private void OnInitInstanceComplete(BaseMessage message)
        {
            if (message is InitInstanceCompleteMessage m)
            {
                if (m.IsSuccess)
                {
                    initTask.SetResult(true);
                    IsInitialized = true;
                }
                else
                    initTask.SetException(m.Exception);
            }
        }
        #endregion
    }
}
