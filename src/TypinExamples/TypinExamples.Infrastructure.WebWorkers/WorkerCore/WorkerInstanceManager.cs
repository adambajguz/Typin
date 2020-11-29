namespace TypinExamples.Infrastructure.WebWorkers.WorkerCore
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;
    using TypinExamples.Infrastructure.WebWorkers.Common.Messaging;
    using TypinExamples.Infrastructure.WebWorkers.Common.Payloads;
    using TypinExamples.Infrastructure.WebWorkers.Core.Internal;

    public class WorkerInstanceManager
    {
        private readonly IdProvider _idProvider = new();

        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private readonly ISerializer _serializer;

        private ServiceProvider? ServiceProvider { get; set; }
        private readonly Dictionary<Type, Action<IMessage>> _messageHandlerRegistry = new();
        private readonly Dictionary<ulong, TaskCompletionSource<object>> messageRegister = new();

        public ulong Id { get; }

        public WorkerInstanceManager(ulong id, ISerializer? serializer = null)
        {
            Id = id;

            _serializer = serializer ?? new DefaultSerializer();
            MessageService.Message += OnMessage;

            _messageHandlerRegistry.Add(typeof(Message<Dispose.Payload>), DisposeInstance);
            _messageHandlerRegistry.Add(typeof(Message<Cancel.Payload>), HandleCancel);
            _messageHandlerRegistry.Add(typeof(Message<RunProgram.Payload>), HandleRunProgram);
        }

        #region Messaging
        public void PostMessage<TMessage>(TMessage message)
            where TMessage : IMessage
        {
            string? serialized = _serializer.Serialize(message);

            MessageService.PostMessage(serialized);
        }

        private async Task<TResultPayload?> PostMessageAsync<TPayload, TResultPayload>(TPayload payload)
        {
            var callId = _idProvider.Next();

            var taskCompletionSource = new TaskCompletionSource<object>();
            messageRegister.Add(callId, taskCompletionSource);

            Message<TPayload> message = new()
            {
                Id = callId,
                WorkerId = Id,
                Payload = payload
            };

            PostMessage(message);

            if (await taskCompletionSource.Task is not Message<TResultPayload> returnMessage)
                throw new InvalidOperationException("Invalid message.");

            if (returnMessage.Exception is not null)
                throw new AggregateException($"Worker exception: {returnMessage.Exception.Message}", returnMessage.Exception);

            return returnMessage.Payload;
        }

        private void OnMessage(object? sender, string rawMessage)
        {
            IMessage message = _serializer.Deserialize<IMessage>(rawMessage);

            if (_messageHandlerRegistry.TryGetValue(message.GetType(), out var value))
                value.Invoke(message);
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

            PostMessage(new Message<Init.ResultPayload>
            {
                Id = 0,
                WorkerId = Id,
                Payload = new Init.ResultPayload()
            });
        }

        public void HandleCancel(IMessage message)
        {
            if (message is Message<Cancel.Payload> cancel)
            {
                _cancellationTokenSource.Cancel();

                PostMessage(new Message<Cancel.ResultPayload>
                {
                    Id = 2,
                    WorkerId = Id,
                    Payload = new Cancel.ResultPayload()
                });
            }
        }

        private void HandleRunProgram(IMessage message)
        {
            if (message is Message<RunProgram.Payload> methodCallMessage)
            {
                void handleError(Exception e)
                {
                    PostMessage(new Message<RunProgram.ResultPayload>
                    {
                        Id = 1,
                        WorkerId = Id,
                        Payload = new RunProgram.ResultPayload { },
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
                            handleError(t.Exception!);
                        else
                        {
                            int ec = t.Result;

                            PostMessage(new Message<RunProgram.ResultPayload>
                            {
                                Id = 1,
                                WorkerId = Id,
                                Payload = new RunProgram.ResultPayload
                                {
                                    ExitCode = ec
                                },
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
            if (message is Message<Dispose.Payload> dispose)
            {
                _cancellationTokenSource.Cancel();
                ServiceProvider?.Dispose();
                _cancellationTokenSource.Dispose();

                PostMessage(new Message<Dispose.ResultPayload>
                {
                    Id = 2,
                    WorkerId = Id,
                    Payload = new Dispose.ResultPayload
                    {
                        IsSuccess = true,
                    },
                });

                MessageService.Message -= OnMessage;
            }
        }
    }
}
