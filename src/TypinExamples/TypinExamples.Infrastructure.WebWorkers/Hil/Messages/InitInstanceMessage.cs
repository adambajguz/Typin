namespace TypinExamples.Infrastructure.WebWorkers.Hil
{
    using TypinExamples.Infrastructure.WebWorkers.Hil.Messages.Base;

    public class InitInstanceMessage : BaseMessage
    {
        public ulong WorkerId { get; init; }
        public long InstanceId { get; init; }

        public string? StartupType { get; init; }

        public long CallId { get; init; }
    }
}
