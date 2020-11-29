namespace TypinExamples.Infrastructure.WebWorkers.Core.Internal.Messaging
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;
    using TypinExamples.Infrastructure.WebWorkers.Common.Messaging;

    internal class MessageHandlerWrapper<TRequest, TResponse> : IMessageHandlerWrapper
    {
        private readonly IMessageHandler<TRequest, TResponse> _handler;

        public MessageHandlerWrapper(IMessageHandler<TRequest, TResponse> handler)
        {
            _handler = handler;
        }

        public async Task<IMessage> Handle(IMessage message, CancellationToken cancellationToken)
        {
            try
            {
                IMessage<TRequest>? casted = message as IMessage<TRequest>;
                TResponse response = await _handler.HandleAsync(casted.Payload, cancellationToken);

                return new Message<TResponse>
                {
                    Id = message.Id,
                    WorkerId = message.WorkerId,
                    FromWorker = true,
                    IsResult = true,
                    Payload = response,
                };
            }
            catch (Exception ex)
            {
                return new Message<TResponse>
                {
                    Id = message.Id,
                    WorkerId = message.WorkerId,
                    FromWorker = true,
                    IsResult = true,
                    Exception = ex
                };
            }
        }
    }

    internal class MessageHandlerWrapper<TRequest> : INoResultMessageHandlerWrapper
    {
        private readonly IMessageHandler<TRequest> _handler;

        public MessageHandlerWrapper(IMessageHandler<TRequest> handler)
        {
            _handler = handler;
        }

        public async Task Handle(IMessage message, CancellationToken cancellationToken)
        {
            try
            {
                IMessage<TRequest>? casted = message as IMessage<TRequest>;
                await _handler.HandleAsync(casted.Payload, cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //TODO: exception catching?
                throw;
            }
        }
    }
}
