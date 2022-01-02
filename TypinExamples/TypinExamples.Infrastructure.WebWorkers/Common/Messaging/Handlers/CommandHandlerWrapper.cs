﻿namespace TypinExamples.Infrastructure.WebWorkers.Common.Messaging.Handlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Extensions;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Payloads;
    using TypinExamples.Infrastructure.WebWorkers.Common.Messaging;

    internal class CommandHandlerWrapper<TCommand> : CommandHandlerWrapper<TCommand, CommandFinished>
        where TCommand : ICommand
    {
        public CommandHandlerWrapper(ICommandHandler<TCommand> handler) : base(handler)
        {

        }
    }

    internal class CommandHandlerWrapper<TCommand, TResult> : ICommandHandlerWrapper
        where TCommand : ICommand<TResult>
    {
        private readonly ICommandHandler<TCommand, TResult> _handler;

        public CommandHandlerWrapper(ICommandHandler<TCommand, TResult> handler)
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
                {
                    throw new InvalidOperationException("Cannot handle message that is not a command call.");
                }

                Message<TCommand> casted = message as Message<TCommand> ?? throw new NullReferenceException("Invalid command message type.");
                _ = casted.Payload ?? throw new NullReferenceException($"Invalid payload type in {casted.Type}");

                TResult response = await _handler.HandleAsync(casted.Payload, worker, cancellationToken);

                return new Message<TResult>
                {
                    Id = message.Id,
                    WorkerId = message.TargetWorkerId,
                    TargetWorkerId = message.WorkerId,
                    Type = messageFrom | MessageTypes.Result,
                    Payload = response,
                };
            }
            catch (Exception ex)
            {
                return new Message<TResult>
                {
                    Id = message.Id,
                    WorkerId = message.TargetWorkerId,
                    TargetWorkerId = message.WorkerId,
                    Type = messageFrom | MessageTypes.Exception,
                    Error = WorkerError.FromException(ex)
                };
            }
        }
    }
}
