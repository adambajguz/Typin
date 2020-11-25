namespace TypinExamples.Infrastructure.WebWorkers.Hil
{
    public class MethodCallParams : BaseMessage
    {
        public MethodCallParams()
        {
            MessageType = nameof(MethodCallParams);
        }

        public long InstanceId { get; set; }
        public string ProgramClass { get; set; }
        public long WorkerId { get; set; }
        public long CallId { get; set; }
    }
}
