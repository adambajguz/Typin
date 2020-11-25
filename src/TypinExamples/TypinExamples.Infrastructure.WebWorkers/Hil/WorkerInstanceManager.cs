namespace TypinExamples.Infrastructure.WebWorkers.Hil
{
    using System;
    using System.Threading.Tasks;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;
    using TypinExamples.Infrastructure.WebWorkers.WorkerCore;
    using TypinExamples.Infrastructure.WebWorkers.WorkerCore.SimpleInstanceService;
    using TypinExamples.Infrastructure.WebWorkers.WorkerCore.SimpleInstanceService.Messages;

    public partial class WorkerInstanceManager
    {
        public static readonly WorkerInstanceManager Instance = new WorkerInstanceManager();
        internal readonly ISerializer serializer;
        private readonly WebWorkerOptions options;
        private readonly MessageHandlerRegistry messageHandlerRegistry;
        private readonly SimpleInstanceService simpleInstanceService;

        public WorkerInstanceManager()
        {
            serializer = new DefaultMessageSerializer();
            options = new WebWorkerOptions();
            simpleInstanceService = SimpleInstanceService.Instance;

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

        private bool IsInfrastructureMessage(string message)
        {
            return messageHandlerRegistry.HandlesMessage(message);
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
                    IsException = true,
                    Exception = e
                });
            }

            try
            {
                Task.Run(async () =>
                    await MethodCall(methodCallMessage))
                    .ContinueWith(t =>
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

        public void InitInstance(InitInstanceMessage createInstanceInfo)
        {
            var initResult = simpleInstanceService.InitInstance(
                new InitInstanceRequest
                {
                    Id = createInstanceInfo.InstanceId,
                    TypeName = createInstanceInfo.TypeName,
                    AssemblyName = createInstanceInfo.AssemblyName
                }, IsInfrastructureMessage);

            PostObject(new InitInstanceCompleteMessage()
            {
                CallId = createInstanceInfo.CallId,
                IsSuccess = initResult.IsSuccess,
                Exception = initResult.Exception,
            });
        }

        public void DisposeInstance(DisposeInstanceMessage dispose)
        {
            var res = simpleInstanceService.DisposeInstance(
                new DisposeInstanceRequest
                {
                    InstanceId = dispose.InstanceId,
                    CallId = dispose.CallId
                });

            PostObject(new DisposeInstanceCompleteMessage
            {
                CallId = res.CallId,
                IsSuccess = res.IsSuccess,
                Exception = res.Exception
            });
        }

        public async Task<object> MethodCall(MethodCallParamsMessage instanceMethodCallParams)
        {
            IWebWorkerEntryPoint? instance = (IWebWorkerEntryPoint)simpleInstanceService.instances[instanceMethodCallParams.InstanceId].Instance;

            return await instance.Main();
        }

        public static async Task<int?> RunExample(string programClass)
        {
            Type? type = Type.GetType(programClass);

            Task<int>? task = type?.GetMethod("Main")?.Invoke(null, null) as Task<int>;

            if (task is null)
                return null;

            int? exitCode = await task;

            return exitCode;
        }
    }
}
