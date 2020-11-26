namespace TypinExamples.Infrastructure.WebWorkers.Hil
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;
    using TypinExamples.Infrastructure.WebWorkers.Hil.Messages.Base;

    public class WorkerBackgroundServiceProxy<T> where T : class
    {
        private readonly IWorker worker;
        private static readonly string InitEndPoint;
        private static long idSource;
        private readonly long instanceId;
        private readonly ISerializer _serializer;
        private readonly Dictionary<Type, Action<BaseMessage>> _messageHandlerRegistry = new();
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
            _serializer = serializer ??= new DefaultSerializer();

            _messageHandlerRegistry.Add(typeof(InitInstanceCompleteMessage), OnInitInstanceComplete);
            _messageHandlerRegistry.Add(typeof(InitWorkerCompleteMessage), OnInitWorkerComplete);
            _messageHandlerRegistry.Add(typeof(DisposeInstanceCompleteMessage), OnDisposeInstanceComplete);
            _messageHandlerRegistry.Add(typeof(MethodCallResultMessage), OnMethodCallResult);
        }

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
            BaseMessage message = _serializer.Deserialize<BaseMessage>(rawMessage);

            if (_messageHandlerRegistry.TryGetValue(message.GetType(), out var value))
            {
                value.Invoke(message);
            }
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

            return returnMessage.ExitCode;
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
