namespace TypinExamples.Infrastructure.WebWorkers.Hil
{
    using TypinExamples.Infrastructure.WebWorkers.Hil.Messages.Base;

    public class MethodCallResultMessage : BaseMessage
    {
        public string ResultPayload { get; init; }
        public long CallId { get; init; }
    }
}
