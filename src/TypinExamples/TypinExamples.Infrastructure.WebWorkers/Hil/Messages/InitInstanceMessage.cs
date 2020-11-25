namespace TypinExamples.Infrastructure.WebWorkers.Hil
{
    using TypinExamples.Infrastructure.WebWorkers.Hil.Messages.Base;

    public class InitInstanceMessage : BaseMessage
    {
        public InitInstanceMessage()
        {
            MessageType = nameof(InitInstanceMessage);
        }

        public long WorkerId { get; set; }
        public long InstanceId { get; set; }
        public string AssemblyName { get; set; }
        public string TypeName { get; set; }

        public long CallId { get; set; }
    }
}
