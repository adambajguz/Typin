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
                    .AddScoped<ISerializer, DefaultSerializer>()
                    .AddSingleton(new WorkerIdAccessor())
                    .AddTransient(typeof(NotificationHandlerWrapper<>))
                    .AddTransient(typeof(CommandHandlerWrapper<>))
                    .AddTransient(typeof(CommandHandlerWrapper<,>))
                    .AddScoped<IMessagingProvider, MainThreadMessagingProvider>()
                    .AddScoped<IMessagingService, MainMessagingService>();

            return services;
        }

        public static IServiceCollection RegisterNotificationHandler<TPayload, THandler>(this IServiceCollection services)
            where THandler : INotificationHandler<TPayload>
        {
            Type messageType = typeof(Message<TPayload>);

            MessageMapping mapping = new MessageMapping(messageType,
                                                        typeof(TPayload),
                                                        typeof(THandler),
                                                        typeof(INotificationHandler<TPayload>),
                                                        typeof(NotificationHandlerWrapper<TPayload>));

            MainConfiguration.AddMapping(mapping);
            services.TryAddTransient(mapping.HandlerInterfaceType, mapping.HandlerType);

            return services;
        }

        public static IServiceCollection RegisterCommandHandler<TPayload, THandler>(this IServiceCollection services)
            where THandler : ICommandHandler<TPayload>
        {
            Type messageType = typeof(Message<TPayload>);

            MessageMapping mapping = new MessageMapping(messageType,
                                                        typeof(TPayload),
                                                        typeof(CommandFinished),
                                                        typeof(THandler),
                                                        typeof(ICommandHandler<TPayload>),
                                                        typeof(CommandHandlerWrapper<TPayload>));

            MainConfiguration.AddMapping(mapping);
            services.TryAddTransient(mapping.HandlerInterfaceType, mapping.HandlerType);

            return services;
        }

        public static IServiceCollection RegisterCommandHandler<TPayload, THandler, TResultPayload>(this IServiceCollection services)
            where THandler : ICommandHandler<TPayload, TResultPayload>
        {
            Type messageType = typeof(Message<TPayload>);

            MessageMapping mapping = new MessageMapping(messageType,
                                                        typeof(TPayload),
                                                        typeof(TResultPayload),
                                                        typeof(THandler),
                                                        typeof(ICommandHandler<TPayload, TResultPayload>),
                                                        typeof(CommandHandlerWrapper<TPayload, TResultPayload>));

            MainConfiguration.AddMapping(mapping);
            services.TryAddTransient(mapping.HandlerInterfaceType, mapping.HandlerType);

            return services;
        }
    }
}
