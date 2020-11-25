namespace TypinExamples.Infrastructure.WebWorkers.Hil
{
    using System;
    using TypinExamples.Infrastructure.WebWorkers.Hil.Messages.Base;

    public class MethodCallResultMessage : BaseMessage
    {
        public MethodCallResultMessage()
        {
            MessageType = nameof(MethodCallResultMessage);
        }

        public string ResultPayload { get; set; }

        public bool IsException { get; set; }

        public Exception Exception { get; set; }

        public long CallId { get; set; }
    }
}
