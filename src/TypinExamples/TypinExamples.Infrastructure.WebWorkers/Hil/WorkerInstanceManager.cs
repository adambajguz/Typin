namespace TypinExamples.Infrastructure.WebWorkers.Hil
{
    using System;
    using System.Threading.Tasks;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;
    using TypinExamples.Infrastructure.WebWorkers.WorkerCore;

    public partial class WorkerInstanceManager
    {
        public static readonly WorkerInstanceManager Instance = new WorkerInstanceManager();
        internal readonly ISerializer serializer;
        private readonly WebWorkerOptions options;
        private readonly MessageHandlerRegistry messageHandlerRegistry;

        public WorkerInstanceManager()
        {
            serializer = new DefaultMessageSerializer();
            options = new WebWorkerOptions();

            messageHandlerRegistry = new MessageHandlerRegistry(serializer);
            messageHandlerRegistry.Add<InitInstanceMessage>(InitInstance);
            messageHandlerRegistry.Add<DisposeInstanceMessage>(DisposeInstance);
            messageHandlerRegistry.Add<MethodCallParamsMessage>(HandleMethodCall);
        }

        public static void Init()
        {
            MessageService.Message += Instance.OnMessage;
            Instance.PostObject(new InitWorkerCompleteMessage());
#if DEBUG
            Console.WriteLine($"BlazorWorker.WorkerBackgroundService.{nameof(WorkerInstanceManager)}.Init(): Done.");
#endif
        }

        public void PostMessage(string message)
        {
#if DEBUG
            Console.WriteLine($"BlazorWorker.WorkerBackgroundService.{nameof(WorkerInstanceManager)}.PostMessage(): {message}.");
#endif
            MessageService.PostMessage(message);
        }

        internal void PostObject<T>(T obj)
        {
            PostMessage(serializer.Serialize(obj));
        }

        private void OnMessage(object sender, string message)
        {
            messageHandlerRegistry.HandleMessage(message);
        }

        private void HandleMethodCall(MethodCallParamsMessage methodCallMessage)
        {
            void handleError(Exception e)
            {
                PostObject(
                new MethodCallResultMessage()
                {
                    CallId = methodCallMessage.CallId,
                    Exception = e
                });
            }

            try
            {
                Task.Run(async () =>
                {
                    return await MethodCall(methodCallMessage);
                }).ContinueWith(t =>
                {
                    if (t.IsFaulted)
                        handleError(t.Exception);
                    else
                        PostObject(
                            new MethodCallResultMessage
                            {
                                CallId = methodCallMessage.CallId,
                                ResultPayload = serializer.Serialize(t.Result)
                            }
                        );
                });
            }
            catch (Exception e)
            {
                handleError(e);
            }
        }

        private IWebWorkerEntryPoint _instance;

        public void InitInstance(InitInstanceMessage createInstanceInfo)
        {
            Type? type = Type.GetType(createInstanceInfo.Type);

            _instance = Activator.CreateInstance(type) as IWebWorkerEntryPoint;

            PostObject(new InitInstanceCompleteMessage()
            {
                CallId = createInstanceInfo.CallId,
                IsSuccess = _instance is not null,
                Exception = null,
            });
        }

        public async void DisposeInstance(DisposeInstanceMessage dispose)
        {
            if (_instance is IDisposable d)
            {
                d.Dispose();
            }
            else if (_instance is IAsyncDisposable ad)
            {
                await ad.DisposeAsync();
            }

            PostObject(new DisposeInstanceCompleteMessage
            {
                CallId = dispose.CallId,
                IsSuccess = true,
                Exception = null
            });
        }

        public async Task<object> MethodCall(MethodCallParamsMessage instanceMethodCallParams)
        {
            return await _instance.Main();
        }
    }
}
