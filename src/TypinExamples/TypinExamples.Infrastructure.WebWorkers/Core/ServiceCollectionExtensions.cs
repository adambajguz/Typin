namespace TypinExamples.Infrastructure.WebWorkers.Core
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Payloads;
    using TypinExamples.Infrastructure.WebWorkers.Common.Messaging;
    using TypinExamples.Infrastructure.WebWorkers.Common.Messaging.Handlers;
    using TypinExamples.Infrastructure.WebWorkers.Core.Internal;
    using TypinExamples.Infrastructure.WebWorkers.Core.Internal.JS;
    using TypinExamples.Infrastructure.WebWorkers.Core.Internal.Messaging;

    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds <see cref="IWorkerFactory"/> as a singleton service
        /// to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddWebWorkers(this IServiceCollection services)
        {
            services.AddScoped<IWorkerFactory, WorkerFactory>()
                    .AddScoped<IWorkerManager, WorkerManager>()
                    .AddScoped<ISerializer, DefaultSerializer>()
                    .AddScoped<IScriptLoader, ScriptLoader>()
                    .AddSingleton(new WorkerIdAccessor())
                    .AddTransient(typeof(NotificationHandlerWrapper<>))
                    .AddTransient(typeof(CommandHandlerWrapper<>))
                    .AddTransient(typeof(CommandHandlerWrapper<,>))
                    .AddScoped<IMessagingProvider, MainThreadMessagingProvider>()
                    .AddScoped<IMessagingService, MainMessagingService>();

            return services;
        }

        public static IServiceCollection RegisterNotificationHandler<TNotification, THandler>(this IServiceCollection services)
            where THandler : INotificationHandler<TNotification>
            where TNotification : INotification
        {
            Type messageType = typeof(Message<TNotification>);

            MessageMapping mapping = new MessageMapping(messageType,
                                                        typeof(TNotification),
                                                        typeof(THandler),
                                                        typeof(INotificationHandler<TNotification>),
                                                        typeof(NotificationHandlerWrapper<TNotification>));

            MainConfiguration.AddMapping(mapping);
            services.TryAddTransient(mapping.HandlerInterfaceType, mapping.HandlerType);

            return services;
        }

        public static IServiceCollection RegisterCommandHandler<TCommand, THandler>(this IServiceCollection services)
            where THandler : ICommandHandler<TCommand>
            where TCommand : ICommand
        {
            Type messageType = typeof(Message<TCommand>);

            MessageMapping mapping = new MessageMapping(messageType,
                                                        typeof(TCommand),
                                                        typeof(CommandFinished),
                                                        typeof(THandler),
                                                        typeof(ICommandHandler<TCommand>),
                                                        typeof(CommandHandlerWrapper<TCommand>));

            MainConfiguration.AddMapping(mapping);
            services.TryAddTransient(mapping.HandlerInterfaceType, mapping.HandlerType);

            return services;
        }

        public static IServiceCollection RegisterCommandHandler<TCommand, THandler, TResult>(this IServiceCollection services)
            where THandler : ICommandHandler<TCommand, TResult>
            where TCommand : ICommand<TResult>
        {
            Type messageType = typeof(Message<TCommand>);

            MessageMapping mapping = new MessageMapping(messageType,
                                                        typeof(TCommand),
                                                        typeof(TResult),
                                                        typeof(THandler),
                                                        typeof(ICommandHandler<TCommand, TResult>),
                                                        typeof(CommandHandlerWrapper<TCommand, TResult>));

            MainConfiguration.AddMapping(mapping);
            services.TryAddTransient(mapping.HandlerInterfaceType, mapping.HandlerType);

            return services;
        }
    }
}
