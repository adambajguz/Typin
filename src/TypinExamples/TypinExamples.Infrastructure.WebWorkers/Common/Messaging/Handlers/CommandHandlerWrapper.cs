namespace TypinExamples.Infrastructure.WebWorkers.Common.Messaging.Handlers
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
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

        public async Task<IMessage> Handle(IMessage message, CancellationToken cancellationToken)
        {
            var timer = new Stopwatch();
            timer.Start();

            bool isFromMain = message.Type.HasFlags(MessageTypes.FromMain);
            MessageTypes messageFrom = isFromMain ? MessageTypes.FromWorker : MessageTypes.FromMain;

            try
            {
                Message<TRequest>? casted = message as Message<TRequest>;
                TResponse response = await _handler.HandleAsync(casted.Payload, cancellationToken);

                timer.Stop();

                return new Message<TResponse>
                {
                    Id = message.Id,
                    WorkerId = isFromMain ? message.TargetWorkerId : null,
                    TargetWorkerId = isFromMain ? null : message.TargetWorkerId,
                    Type = messageFrom | MessageTypes.Result,
                    Payload = response,
                };
            }
            catch (Exception ex)
            {
                timer.Stop();

                return new Message<TResponse>
                {
                    Id = message.Id,
                    WorkerId = isFromMain ? message.TargetWorkerId : null,
                    TargetWorkerId = isFromMain ? null : message.TargetWorkerId,
                    Type = messageFrom | MessageTypes.Result | MessageTypes.Exception,
                    Exception = ex
                };
            }
        }
    }
}
