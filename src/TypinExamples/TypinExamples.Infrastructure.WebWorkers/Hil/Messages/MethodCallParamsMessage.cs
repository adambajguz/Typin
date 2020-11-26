namespace TypinExamples.Infrastructure.WebWorkers.Hil
{
    using TypinExamples.Infrastructure.WebWorkers.Hil.Messages.Base;

    public class MethodCallParamsMessage : BaseMessage
    {
        public long InstanceId { get; init; }
        public string ProgramClass { get; init; }
        public long WorkerId { get; init; }
        public long CallId { get; init; }

        public MethodCallParamsMessage()
        {
            MessageType = nameof(MethodCallParamsMessage);
        }
    }
}
