namespace TypinExamples.Infrastructure.WebWorkers.Hil
{
    using TypinExamples.Infrastructure.WebWorkers.Hil.Messages.Base;

    public class MethodCallParamsMessage : BaseMessage
    {
        public MethodCallParamsMessage()
        {
            MessageType = nameof(MethodCallParamsMessage);
        }

        public long InstanceId { get; set; }
        public string ProgramClass { get; set; }
        public long WorkerId { get; set; }
        public long CallId { get; set; }
    }
}
