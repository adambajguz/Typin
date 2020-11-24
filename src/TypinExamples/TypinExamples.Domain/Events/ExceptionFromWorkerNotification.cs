namespace TypinExamples.Domain.Events
{
    using TypinExamples.Domain.Interfaces.Handlers.Core;

    public class ExceptionFromWorkerNotification : ICoreNotification
    {
        public long? WorkerId { get; set; }
        public string Type { get; init; } = string.Empty;
        public string Message { get; init; } = string.Empty;
        public string? StackTrace { get; init; } = string.Empty;
    }
}
