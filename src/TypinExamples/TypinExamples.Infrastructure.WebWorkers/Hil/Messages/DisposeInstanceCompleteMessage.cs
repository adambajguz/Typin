namespace TypinExamples.Infrastructure.WebWorkers.Hil
{
    using TypinExamples.Infrastructure.WebWorkers.Hil.Messages.Base;

    public class DisposeInstanceCompleteMessage : BaseMessage
    {
        public long CallId { get; init; }
        public bool IsSuccess { get; init; }
    }
}
