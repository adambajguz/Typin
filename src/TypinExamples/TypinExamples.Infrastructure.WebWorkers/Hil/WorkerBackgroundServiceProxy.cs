namespace TypinExamples.Infrastructure.WebWorkers.Hil
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;

    public class WorkerBackgroundServiceProxy<T> where T : class
    {
        private readonly IWorker worker;
        private static readonly string InitEndPoint;
        private static long idSource;
        private readonly long instanceId;
        private readonly ISerializer _serializer;
        private readonly MessageHandlerRegistry messageHandlerRegistry;
        private TaskCompletionSource<bool> initTask;
        private TaskCompletionSource<bool> disposeTask;
        private TaskCompletionSource<bool> initWorkerTask;

        // This doesnt really need to be static but easier to debug if messages have application-wide unique ids
        private static long messageRegisterIdSource;
        private readonly Dictionary<long, TaskCompletionSource<MethodCallResultMessage>> messageRegister
            = new Dictionary<long, TaskCompletionSource<MethodCallResultMessage>>();

        public bool IsInitialized { get; private set; }
        public bool IsDisposed { get; private set; }

        static WorkerBackgroundServiceProxy()
        {
            var wim = typeof(WorkerInstanceManager);
            InitEndPoint = $"[{wim.Assembly.GetName().Name}]{wim.FullName}:{nameof(WorkerInstanceManager.Init)}";
        }

        public WorkerBackgroundServiceProxy(IWorker worker, ISerializer? serializer = null)
        {
            this.worker = worker;
            instanceId = ++idSource;
            _serializer = serializer ??= new DefaultSerializer(); ;

            messageHandlerRegistry = new MessageHandlerRegistry(_serializer);
            messageHandlerRegistry.Add<InitInstanceCompleteMessage>(OnInitInstanceComplete);
            messageHandlerRegistry.Add<InitWorkerCompleteMessage>(OnInitWorkerComplete);
            messageHandlerRegistry.Add<DisposeInstanceCompleteMessage>(OnDisposeInstanceComplete);
            messageHandlerRegistry.Add<MethodCallResultMessage>(OnMethodCallResult);
        }

        private void OnDisposeInstanceComplete(DisposeInstanceCompleteMessage message)
        {
            if (message.IsSuccess)
            {
                disposeTask.SetResult(true);
                IsDisposed = true;
            }
            else
                disposeTask.SetException(message.Exception);
        }

        public async Task InitAsync()
        {
            if (initTask != null)
                await initTask.Task;

            if (IsInitialized)
                return;

            initTask = new TaskCompletionSource<bool>();

            if (!worker.IsInitialized)
            {
                initWorkerTask = new TaskCompletionSource<bool>();

                await worker.InitAsync(InitEndPoint);

                worker.IncomingMessage += OnMessage;
                await initWorkerTask.Task;
            }

            var message = _serializer.Serialize(
                    new InitInstanceMessage()
                    {
                        WorkerId = worker.Id, // TODO: This should not really be necessary?
                        InstanceId = instanceId,
                        StartupType = typeof(T).AssemblyQualifiedName
                    });
            Console.WriteLine($"{nameof(WorkerBackgroundServiceProxy<T>)}.InitAsync(): {worker.Id} {message}");

            await worker.PostMessageAsync(message);
            await initTask.Task;
        }

        private void OnMessage(object sender, string rawMessage)
        {
            messageHandlerRegistry.HandleMessage(rawMessage);
        }

        private void OnMethodCallResult(MethodCallResultMessage message)
        {
            if (!messageRegister.TryGetValue(message.CallId, out var taskCompletionSource))
                return;

            taskCompletionSource.SetResult(message);
            messageRegister.Remove(message.CallId);
        }

        private void OnInitWorkerComplete(InitWorkerCompleteMessage message)
        {
            initWorkerTask.SetResult(true);
        }

        private void OnInitInstanceComplete(InitInstanceCompleteMessage message)
        {
            if (message.IsSuccess)
            {
                initTask.SetResult(true);
                IsInitialized = true;
            }
            else
                initTask.SetException(message.Exception);
        }

        public async Task<int> RunAsync()
        {
            return await InvokeAsyncInternal();
        }

        private async Task<int> InvokeAsyncInternal()
        {
            // If Blazor ever gets multithreaded this would need to be locked for race conditions
            // However, when/if that happens, most of this project is obsolete anyway
            var id = ++messageRegisterIdSource;
            var taskCompletionSource = new TaskCompletionSource<MethodCallResultMessage>();
            messageRegister.Add(id, taskCompletionSource);

            var methodCallParams = new MethodCallParamsMessage
            {
                WorkerId = worker.Id,
                InstanceId = instanceId,
                ProgramClass = typeof(T).AssemblyQualifiedName ?? throw new ApplicationException($"{typeof(T).Name} is a generic type."),
                CallId = id
            };

            var methodCall = _serializer.Serialize(methodCallParams);

            await worker.PostMessageAsync(methodCall);

            var returnMessage = await taskCompletionSource.Task;

            if (returnMessage.Exception is not null)
                throw new AggregateException($"Worker exception: {returnMessage.Exception.Message}", returnMessage.Exception);

            if (string.IsNullOrEmpty(returnMessage.ResultPayload))
                return default;

            return _serializer.Deserialize<int>(returnMessage.ResultPayload);
        }

        public async ValueTask DisposeAsync()
        {
            if (disposeTask != null)
                await disposeTask.Task;

            if (IsDisposed)
                return;

            disposeTask = new TaskCompletionSource<bool>();

            var message = _serializer.Serialize(
                   new DisposeInstanceMessage
                   {
                       InstanceId = instanceId,
                   });

            await worker.PostMessageAsync(message);

            await disposeTask.Task;
        }
    }
}
