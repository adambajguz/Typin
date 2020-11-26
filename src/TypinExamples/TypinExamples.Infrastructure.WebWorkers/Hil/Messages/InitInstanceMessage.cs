namespace TypinExamples.Infrastructure.WebWorkers.Hil
{
    using System;
    using TypinExamples.Infrastructure.WebWorkers.Hil.Messages.Base;

    public class InitInstanceMessage : BaseMessage
    {
        public long WorkerId { get; init; }
        public long InstanceId { get; init; }
        public string AssemblyName { get; init; }
        public string TypeName { get; init; }
        public string Type { get; init; }

        public long CallId { get; init; }

        public InitInstanceMessage()
        {
            MessageType = nameof(InitInstanceMessage);
        }
    }
}
