namespace TypinExamples.Infrastructure.WebWorkers.Hil
{
    using TypinExamples.Infrastructure.WebWorkers.Hil.Messages.Base;

    public class DisposeInstanceMessage : BaseMessage
    {
        public DisposeInstanceMessage()
        {
            MessageType = nameof(DisposeInstanceMessage);
        }

        public long InstanceId { get; set; }

        public long CallId { get; set; }
    }
}
