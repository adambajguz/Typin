namespace TypinExamples.Infrastructure.WebWorkers.Hil
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;
    using TypinExamples.Infrastructure.WebWorkers.Core;
    using TypinExamples.Infrastructure.WebWorkers.WorkerCore;

    public class WorkerInstanceManager
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        public static readonly WorkerInstanceManager Instance = new WorkerInstanceManager();

        private readonly ISerializer _serializer;

        private ServiceProvider? ServiceProvider { get; set; }
        private readonly Dictionary<Type, Action<IMessage>> _messageHandlerRegistry = new();

        public WorkerInstanceManager(ISerializer? serializer = null)
        {
            _serializer = serializer ?? new DefaultSerializer();

            _messageHandlerRegistry.Add(typeof(InitInstanceMessage), InitInstance);
            _messageHandlerRegistry.Add(typeof(DisposeInstanceMessage), DisposeInstance);
            _messageHandlerRegistry.Add(typeof(CancelMessage), HandleCancel);
            _messageHandlerRegistry.Add(typeof(RunProgramMessage), HandleMethodCall);
        }

        public static void Init()
        {
            MessageService.Message += Instance.OnMessage;
            Instance.PostMessage(new InitWorkerResultMessage());
#if DEBUG
            Console.WriteLine($"BlazorWorker.WorkerBackgroundService.{nameof(WorkerInstanceManager)}.Init(): Done.");
#endif
        }

        public void PostMessage<TMessage>(TMessage message)
            where TMessage : IMessage
        {
            string? serialized = _serializer.Serialize(message);

            MessageService.PostMessage(serialized);
        }

        private void OnMessage(object? sender, string rawMessage)
        {
            IMessage message = _serializer.Deserialize<IMessage>(rawMessage);

            if (_messageHandlerRegistry.TryGetValue(message.GetType(), out var value))
            {
                value.Invoke(message);
            }
        }

        private void HandleMethodCall(IMessage message)
        {
            if (message is RunProgramMessage methodCallMessage)
            {
                void handleError(Exception e)
                {
                    PostMessage(new RunProgramResultMessage()
                    {
                        WorkerId = methodCallMessage.WorkerId,
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
                            PostMessage(new RunProgramResultMessage
                            {
                                WorkerId = methodCallMessage.WorkerId,
                                CallId = methodCallMessage.CallId,
                                ExitCode = t.Result
                            });
                    });
                }
                catch (Exception e)
                {
                    handleError(e);
                }
            }
        }

        public void InitInstance(IMessage message)
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
                serviceCollection.AddTransient(typeof(IWorkerProgram), configuration.DefaultEntryPoint)
                                 .AddTransient<IWorkerMessageService, InjectableMessageService>()
                                 .AddScoped<HttpClient>();

                startup.ConfigureServices(serviceCollection);

                ServiceProvider = serviceCollection.BuildServiceProvider();

                PostMessage(new InitInstanceResultMessage()
                {
                    WorkerId = createInstanceInfo.WorkerId,
                    CallId = createInstanceInfo.CallId,
                    IsSuccess = startup is not null,
                    Exception = null,
                });
            }
        }

        public void DisposeInstance(IMessage message)
        {
            if (message is DisposeInstanceMessage dispose)
            {
                _cancellationTokenSource.Cancel();
                ServiceProvider?.Dispose();
                _cancellationTokenSource.Dispose();

                PostMessage(new DisposeInstanceResultMessage
                {
                    WorkerId = dispose.WorkerId,
                    CallId = dispose.CallId,
                    IsSuccess = true,
                    Exception = null
                });
            }
        }

        public void HandleCancel(IMessage message)
        {
            if (message is CancelMessage cancel)
            {
                _cancellationTokenSource.Cancel();

                PostMessage(new CancelResultMessage
                {
                    WorkerId = cancel.WorkerId,
                    CallId = cancel.CallId,
                });
            }
        }

        public async Task<int> MethodCall(RunProgramMessage instanceMethodCallParams)
        {
            _ = ServiceProvider ?? throw new InvalidOperationException("Worker not initialized.");

            return await ServiceProvider.GetRequiredService<IWorkerProgram>().Main(_cancellationTokenSource.Token);
        }
    }
}
