namespace TypinExamples.Infrastructure.WebWorkers.Common.Messaging.Handlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Extensions;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Payloads;
    using TypinExamples.Infrastructure.WebWorkers.Common.Messaging;

    internal class CommandHandlerWrapper<TRequest> : CommandHandlerWrapper<TRequest, CommandFinished>
    {
        public CommandHandlerWrapper(ICommandHandler<TRequest> handler) : base(handler)
        {

        }
    }

    internal class CommandHandlerWrapper<TRequest, TResponse> : ICommandHandlerWrapper
    {
        private readonly ICommandHandler<TRequest, TResponse> _handler;

        public CommandHandlerWrapper(ICommandHandler<TRequest, TResponse> handler)
        {
            _handler = handler;
        }

        public async Task<IMessage> Handle(IMessage message, IWorker worker, CancellationToken cancellationToken)
        {
            bool isFromMain = message.Type.HasFlags(MessageTypes.FromMain);
            bool isCommand = message.Type.HasFlags(MessageTypes.Command);

            MessageTypes messageFrom = isFromMain ? MessageTypes.FromWorker : MessageTypes.FromMain;

            try
            {
                if (!isCommand)
                    throw new InvalidOperationException("Cannot handle message that is not a command call.");

                Message<TRequest>? casted = message as Message<TRequest>;
                TResponse response = await _handler.HandleAsync(casted.Payload, worker, cancellationToken);

                return new Message<TResponse>
                {
                    Id = message.Id,
                    WorkerId = message.TargetWorkerId,
                    TargetWorkerId = message.WorkerId,
                    Type = messageFrom | MessageTypes.Command | MessageTypes.Result,
                    Payload = response,
                };
            }
            catch (Exception ex)
            {
                return new Message<TResponse>
                {
                    Id = message.Id,
                    WorkerId = message.TargetWorkerId,
                    TargetWorkerId = message.WorkerId,
                    Type = messageFrom | MessageTypes.Result | MessageTypes.Exception,
                    Exception = ex
                };
            }
        }
    }
}
