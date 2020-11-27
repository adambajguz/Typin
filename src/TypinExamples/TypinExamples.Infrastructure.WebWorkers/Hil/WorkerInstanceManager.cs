namespace TypinExamples.Infrastructure.WebWorkers.Hil
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;
    using TypinExamples.Infrastructure.WebWorkers.Core.Internal;
    using TypinExamples.Infrastructure.WebWorkers.WorkerCore;
    using TypinExamples.Infrastructure.WebWorkers.WorkerCore.Internal;

    public class WorkerInstanceManager
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private readonly ISerializer _serializer;

        private ServiceProvider? ServiceProvider { get; set; }
        private readonly Dictionary<Type, Action<IMessage>> _messageHandlerRegistry = new();

        public ulong Id { get; }

        public WorkerInstanceManager(ulong id, ISerializer? serializer = null)
        {
            Id = id;

            _serializer = serializer ?? new DefaultSerializer();
            MessageService.Message += OnMessage;

            _messageHandlerRegistry.Add(typeof(DisposeInstanceMessage), DisposeInstance);
            _messageHandlerRegistry.Add(typeof(CancelMessage), HandleCancel);
            _messageHandlerRegistry.Add(typeof(RunProgramMessage), HandleRunProgram);
        }

        #region Messages
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
        #endregion

        public void Start(string? startupType)
        {
            if (string.IsNullOrWhiteSpace(startupType))
                throw new ArgumentException($"'{nameof(startupType)}' cannot be null or whitespace", nameof(startupType));

            //Create startup class
            Type type = Type.GetType(startupType) ?? throw new InvalidOperationException("Invalid startup class type.");
            IWorkerStartup startup = Activator.CreateInstance(type) as IWorkerStartup ?? throw new InvalidOperationException("Invalid startup class.");

            //Build configuration and service collection
            WorkerConfigurationBuilder configurationBuilder = new();
            startup.Configure(configurationBuilder);

            WorkerConfiguration configuration = configurationBuilder.Build();

            ServiceCollection serviceCollection = new ServiceCollection();
            startup.ConfigureServices(serviceCollection);

            serviceCollection.AddTransient(typeof(IWorkerProgram), configuration.ProgramType)
                             .AddTransient<IWorkerMessageService, InjectableMessageService>()
                             .AddSingleton(configuration)
                             .AddSingleton(new WorkerIdAccessor(Id))
                             .AddScoped<HttpClient>();

            ServiceProvider = serviceCollection.BuildServiceProvider();

            PostMessage(new InitWorkerResultMessage());
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

        private void HandleRunProgram(IMessage message)
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
                        _ = ServiceProvider ?? throw new InvalidOperationException("Worker not initialized.");

                        return await ServiceProvider.GetRequiredService<IWorkerProgram>().Main(_cancellationTokenSource.Token);
                    }).ContinueWith(t =>
                    {
                        if (t.IsFaulted)
                        {
                            handleError(t.Exception!);
                        }
                        else
                        {
                            PostMessage(new RunProgramResultMessage
                            {
                                WorkerId = methodCallMessage.WorkerId,
                                CallId = methodCallMessage.CallId,
                                ExitCode = t.Result
                            });
                        }
                    });
                }
                catch (Exception e)
                {
                    handleError(e);
                }
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

                MessageService.Message -= OnMessage;
            }
        }
    }
}
