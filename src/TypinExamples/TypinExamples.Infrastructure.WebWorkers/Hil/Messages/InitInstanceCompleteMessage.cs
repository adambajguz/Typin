namespace TypinExamples.Infrastructure.WebWorkers.Hil
{
    using System;
    using TypinExamples.Infrastructure.WebWorkers.Hil.Messages.Base;

    public class InitInstanceCompleteMessage : BaseMessage
    {
        public InitInstanceCompleteMessage()
        {
            MessageType = nameof(InitInstanceCompleteMessage);
        }

        public long CallId { get; set; }

        public bool IsSuccess { get; set; }

        public Exception Exception { get; set; }
    }
}
