namespace TypinExamples.Infrastructure.WebWorkers.Hil
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;
    using TypinExamples.Infrastructure.WebWorkers.Core;
    using TypinExamples.Infrastructure.WebWorkers.Hil.Messages.Base;
    using TypinExamples.Infrastructure.WebWorkers.WorkerCore;

    public class WorkerInstanceManager
    {
        public static readonly WorkerInstanceManager Instance = new WorkerInstanceManager();

        private readonly ISerializer _serializer;

        private ServiceProvider? ServiceProvider { get; set; }
        private readonly Dictionary<Type, Action<BaseMessage>> _messageHandlerRegistry = new();

        public WorkerInstanceManager(ISerializer? serializer = null)
        {
            _serializer = serializer ?? new DefaultSerializer();

            _messageHandlerRegistry.Add(typeof(InitInstanceMessage), InitInstance);
            _messageHandlerRegistry.Add(typeof(DisposeInstanceMessage), DisposeInstance);
            _messageHandlerRegistry.Add(typeof(MethodCallParamsMessage), HandleMethodCall);
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
            PostMessage(_serializer.Serialize(obj));
        }

        private void OnMessage(object sender, string rawMessage)
        {
            BaseMessage message = _serializer.Deserialize<BaseMessage>(rawMessage);

            if (_messageHandlerRegistry.TryGetValue(message.GetType(), out var value))
            {
                value.Invoke(message);
            }
        }

        private void HandleMethodCall(BaseMessage message)
        {
            if (message is MethodCallParamsMessage methodCallMessage)
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
                                    ExitCode = t.Result
                                }
                            );
                    });
                }
                catch (Exception e)
                {
                    handleError(e);
                }
            }
        }

        public void InitInstance(BaseMessage message)
        {
            if (message is InitInstanceMessage createInstanceInfo)
            {
                Type type = Type.GetType(createInstanceInfo.StartupType) ?? throw new InvalidOperationException("Invalid startup class type.");
                IWorkerStartup startup = Activator.CreateInstance(type) as IWorkerStartup ?? throw new InvalidOperationException("Invalid startup class.");

                ServiceCollection serviceCollection = new ServiceCollection();

                WorkerConfigurationBuilder configurationBuilder = new();
                startup.Configure(configurationBuilder);

                WorkerConfiguration? configuration = configurationBuilder.Build();
                startup.ConfigureServices(serviceCollection);
                serviceCollection.AddTransient(typeof(IWorkerProgram), configuration.DefaultEntryPoint);

                startup.ConfigureServices(serviceCollection);

                ServiceProvider = serviceCollection.BuildServiceProvider();

                PostObject(new InitInstanceCompleteMessage()
                {
                    CallId = createInstanceInfo.CallId,
                    IsSuccess = startup is not null,
                    Exception = null,
                });
            }
        }

        public void DisposeInstance(BaseMessage message)
        {
            if (message is DisposeInstanceMessage dispose)
            {
                ServiceProvider?.Dispose();

                PostObject(new DisposeInstanceCompleteMessage
                {
                    CallId = dispose.CallId,
                    IsSuccess = true,
                    Exception = null
                });
            }
        }

        public async Task<int> MethodCall(MethodCallParamsMessage instanceMethodCallParams)
        {
            _ = ServiceProvider ?? throw new InvalidOperationException("Worker not initialized.");

            return await ServiceProvider.GetRequiredService<IWorkerProgram>().Main();
        }
    }
}
