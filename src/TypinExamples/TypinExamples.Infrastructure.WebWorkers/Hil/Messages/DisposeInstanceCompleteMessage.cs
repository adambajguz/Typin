namespace TypinExamples.Infrastructure.WebWorkers.Hil
{
    using System;
    using TypinExamples.Infrastructure.WebWorkers.Hil.Messages.Base;

    public class DisposeInstanceCompleteMessage : BaseMessage
    {
        public DisposeInstanceCompleteMessage()
        {
            MessageType = nameof(DisposeInstanceCompleteMessage);
        }

        public long CallId { get; set; }

        public bool IsSuccess { get; set; }

        public Exception Exception { get; set; }
    }
}
