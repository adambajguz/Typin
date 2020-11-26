namespace TypinExamples.Infrastructure.WebWorkers.Hil
{
    using System;
    using TypinExamples.Infrastructure.WebWorkers.Hil.Messages.Base;

    public class InitInstanceCompleteMessage : BaseMessage
    {
        public long CallId { get; init; }
        public bool IsSuccess { get; init; }
        public Exception? Exception { get; init; }

        public InitInstanceCompleteMessage()
        {
            MessageType = nameof(InitInstanceCompleteMessage);
        }
    }
}
