namespace BlazorWorker.BackgroundServiceFactory
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using BlazorWorker.Core;
    using BlazorWorker.WorkerBackgroundService;
    using BlazorWorker.WorkerCore;

    public class WorkerBackgroundServiceProxy<T> : IWorkerBackgroundService<T> where T : class
    {
        private readonly IWorker worker;
        private readonly WebWorkerOptions options;
        private static readonly string InitEndPoint;
        private static long idSource;
        private readonly long instanceId;
        private readonly ISerializer messageSerializer;
        private readonly object expressionSerializer;
        private readonly MessageHandlerRegistry messageHandlerRegistry;
        private TaskCompletionSource<bool> initTask;
        private TaskCompletionSource<bool> disposeTask;
        private TaskCompletionSource<bool> initWorkerTask;
        // This doesnt really need to be static but easier to debug if messages have application-wide unique ids
        private static long messageRegisterIdSource;
        private readonly Dictionary<long, TaskCompletionSource<MethodCallResult>> messageRegister
            = new Dictionary<long, TaskCompletionSource<MethodCallResult>>();

        private readonly Dictionary<long, EventHandle> eventRegister
            = new Dictionary<long, EventHandle>();

        public bool IsInitialized { get; private set; }
        public bool IsDisposed { get; private set; }

        static WorkerBackgroundServiceProxy()
        {
            var wim = typeof(WorkerInstanceManager);
            InitEndPoint = $"[{wim.Assembly.GetName().Name}]{wim.FullName}:{nameof(WorkerInstanceManager.Init)}";
        }

        public WorkerBackgroundServiceProxy(
            IWorker worker,
            WebWorkerOptions options)
        {
            this.worker = worker;
            this.options = options;
            instanceId = ++idSource;
            messageSerializer = this.options.MessageSerializer;
            expressionSerializer = this.options.ExpressionSerializer;

            messageHandlerRegistry = new MessageHandlerRegistry(this.options.MessageSerializer);
            messageHandlerRegistry.Add<InitInstanceComplete>(OnInitInstanceComplete);
            messageHandlerRegistry.Add<InitWorkerComplete>(OnInitWorkerComplete);
            messageHandlerRegistry.Add<DisposeInstanceComplete>(OnDisposeInstanceComplete);
            messageHandlerRegistry.Add<EventRaised>(OnEventRaised);
            messageHandlerRegistry.Add<MethodCallResult>(OnMethodCallResult);
        }

        private void OnDisposeInstanceComplete(DisposeInstanceComplete message)
        {
            if (message.IsSuccess)
            {
                disposeTask.SetResult(true);
                IsDisposed = true;
            }
            else
            {
                disposeTask.SetException(message.Exception);
            }
        }

        private bool IsInfrastructureMessage(string message)
        {
            return messageHandlerRegistry.HandlesMessage(message);
        }

        public IWorkerMessageService GetWorkerMessageService()
        {
            return worker;
        }

        public async Task InitAsync(WorkerInitOptions workerInitOptions = null)
        {
            workerInitOptions ??= new WorkerInitOptions();
            if (initTask != null)
            {
                await initTask.Task;
            }

            if (IsInitialized)
            {
                return;
            }

            initTask = new TaskCompletionSource<bool>();

            if (!worker.IsInitialized)
            {
                initWorkerTask = new TaskCompletionSource<bool>();

                if (workerInitOptions.UseConventionalServiceAssembly)
                {
                    workerInitOptions.AddAssemblyOf<T>();
                }

                await worker.InitAsync(new WorkerInitOptions
                {
                    DependentAssemblyFilenames = new[] {
                        $"{typeof(BaseMessage).Assembly.GetName().Name}.dll",
                        $"{typeof(WorkerInstanceManager).Assembly.GetName().Name}.dll",
                        $"{typeof(Newtonsoft.Json.JsonConvert).Assembly.GetName().Name}.dll",
                        $"{typeof(IWorkerMessageService).Assembly.GetName().Name}.dll",
                        $"{typeof(System.Reflection.Assembly).Assembly.GetName().Name}.dll",
                        "System.Xml.dll",
                        "Serialize.Linq.dll",
                        "System.dll",
                        "System.Buffers.dll",
                        "System.Data.dll",
                        "System.Core.dll",
                        "System.Memory.dll",
                        "System.Numerics.dll",
                        "System.Numerics.Vectors.dll",
                        "System.Runtime.CompilerServices.Unsafe.dll",
                        "System.Runtime.Serialization.dll",
                        //"Microsoft.Bcl.AsyncInterfaces.dll",
                        "System.Threading.Tasks.Extensions.dll",
                        "System.Xml.ReaderWriter.dll",
                        "System.Text.RegularExpressions.dll"
                    },
                    InitEndPoint = InitEndPoint
                }.MergeWith(workerInitOptions));

                worker.IncomingMessage += OnMessage;
                await initWorkerTask.Task;
            }

            var message = options.MessageSerializer.Serialize(
                    new InitInstance()
                    {
                        WorkerId = worker.Identifier, // TODO: This should not really be necessary?
                        InstanceId = instanceId,
                        AssemblyName = typeof(T).Assembly.FullName,
                        TypeName = typeof(T).FullName
                    });
            Console.WriteLine($"{nameof(WorkerBackgroundServiceProxy<T>)}.InitAsync(): {worker.Identifier} {message}");

            await worker.PostMessageAsync(message);
            await initTask.Task;
        }

        private void OnMessage(object sender, string rawMessage)
        {
            messageHandlerRegistry.HandleMessage(rawMessage);
        }

        private void OnMethodCallResult(MethodCallResult message)
        {
            if (!messageRegister.TryGetValue(message.CallId, out var taskCompletionSource))
            {
                return;
            }

            taskCompletionSource.SetResult(message);
            messageRegister.Remove(message.CallId);
        }

        private void OnEventRaised(EventRaised message)
        {
            if (!eventRegister.TryGetValue(message.EventHandleId, out var eventHandle))
            {
                Console.WriteLine($"{nameof(WorkerBackgroundServiceProxy<T>)}: {nameof(EventRaised)}: Unknown event id {message.EventHandleId}");
                return;
            }

            OnEventRaised(eventHandle, message.ResultPayload);
        }

        private void OnInitWorkerComplete(InitWorkerComplete message)
        {
            initWorkerTask.SetResult(true);
        }

        private void OnInitInstanceComplete(InitInstanceComplete message)
        {
            if (message.IsSuccess)
            {
                initTask.SetResult(true);
                IsInitialized = true;
            }
            else
            {
                initTask.SetException(message.Exception);
            }
        }

        private void OnEventRaised(EventHandle eventHandle, string eventPayload)
        {
            eventHandle.EventHandler.Invoke(eventPayload);
        }

        public async Task RunAsync(Expression<Action<T>> action)
        {
            await InvokeAsyncInternal<object>(action);
        }

        public async Task<TResult> RunAsync<TResult>(Expression<Func<T, TResult>> action)
        {
            return await InvokeAsyncInternal<TResult>(action);
        }

        public async Task RunAsync<TResult>(Expression<Func<T, Task>> action)
        {
            await InvokeAsyncInternal<object>(action, new InvokeOptions { AwaitResult = true });
        }

        public async Task<TResult> RunAsync<TResult>(Expression<Func<T, Task<TResult>>> function)
        {
            return await InvokeAsyncInternal<TResult>(function, new InvokeOptions { AwaitResult = true });
        }

        public async Task<EventHandle> RegisterEventListenerAsync<TResult>(string eventName, EventHandler<TResult> myHandler)
        {
            var eventSignature = typeof(T).GetEvent(eventName ?? throw new ArgumentNullException(nameof(eventName)));
            if (eventSignature == null)
            {
                throw new ArgumentException($"Type '{typeof(T).FullName}' does not expose any event named '{eventName}'");
            }

            if (!eventSignature.EventHandlerType.IsAssignableFrom(typeof(EventHandler<TResult>)))
            {
                throw new ArgumentException($"Event '{typeof(T).FullName}.{eventName}' can only be assigned an event listener of type {typeof(EventHandler<TResult>).FullName}");
            }

            var handle = new EventHandle()
            {
                EventHandler =
                payload =>
                {
                    var typedPayload = messageSerializer.Deserialize<TResult>(payload);
                    myHandler.Invoke(null, typedPayload);
                }
            };

            eventRegister.Add(handle.Id, handle);
            var message = new RegisterEvent()
            {
                EventName = eventName,
                EventHandlerTypeArg = typeof(TResult).FullName,
                EventHandleId = handle.Id,
                InstanceId = instanceId
            };
            var serializedMessage = options.MessageSerializer.Serialize(message);
            await worker.PostMessageAsync(serializedMessage);
            return handle;
        }

        public async Task UnRegisterEventListenerAsync(EventHandle handle)
        {
            if (handle is null)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            var message = new UnRegisterEvent
            {
                EventHandleId = handle.Id
            };
            var serializedMessage = options.MessageSerializer.Serialize(message);
            await worker.PostMessageAsync(serializedMessage);
        }

        private Task<TResult> InvokeAsyncInternal<TResult>(Expression action)
        {
            return InvokeAsyncInternal<TResult>(action, InvokeOptions.Default);
        }

        private async Task<TResult> InvokeAsyncInternal<TResult>(Expression action, InvokeOptions invokeOptions)
        {
            // If Blazor ever gets multithreaded this would need to be locked for race conditions
            // However, when/if that happens, most of this project is obsolete anyway
            var id = ++messageRegisterIdSource;
            var taskCompletionSource = new TaskCompletionSource<MethodCallResult>();
            messageRegister.Add(id, taskCompletionSource);

            var expression = options.ExpressionSerializer.Serialize(action);
            var methodCallParams = new MethodCallParams
            {
                AwaitResult = invokeOptions.AwaitResult,
                WorkerId = worker.Identifier,
                InstanceId = instanceId,
                SerializedExpression = expression,
                CallId = id
            };

            var methodCall = options.MessageSerializer.Serialize(methodCallParams);

            await worker.PostMessageAsync(methodCall);

            var returnMessage = await taskCompletionSource.Task;
            if (returnMessage.IsException)
            {
                throw new AggregateException($"Worker exception: {returnMessage.Exception.Message}", returnMessage.Exception);
            }
            if (string.IsNullOrEmpty(returnMessage.ResultPayload))
            {
                return default;
            }

            return options.MessageSerializer.Deserialize<TResult>(returnMessage.ResultPayload);
        }

        public async ValueTask DisposeAsync()
        {
            if (disposeTask != null)
            {
                await disposeTask.Task;
            }

            if (IsDisposed)
            {
                return;
            }

            disposeTask = new TaskCompletionSource<bool>();

            var message = options.MessageSerializer.Serialize(
                   new DisposeInstance
                   {
                       InstanceId = instanceId,
                   });

            await worker.PostMessageAsync(message);

            await disposeTask.Task;
        }

        private class InvokeOptions
        {
            public static readonly InvokeOptions Default = new InvokeOptions();

            public bool AwaitResult { get; set; }
        }
    }
}
