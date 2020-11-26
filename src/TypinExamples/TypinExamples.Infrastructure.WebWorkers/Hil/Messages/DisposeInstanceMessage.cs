namespace TypinExamples.Infrastructure.WebWorkers.Hil
{
    using TypinExamples.Infrastructure.WebWorkers.Hil.Messages.Base;

    public class DisposeInstanceMessage : BaseMessage
    {
        public long InstanceId { get; init; }
        public long CallId { get; init; }
    }
}
