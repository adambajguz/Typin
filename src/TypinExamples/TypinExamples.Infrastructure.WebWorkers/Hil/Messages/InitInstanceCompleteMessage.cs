namespace TypinExamples.Infrastructure.WebWorkers.Hil
{
    using TypinExamples.Infrastructure.WebWorkers.Hil.Messages.Base;

    public class InitInstanceCompleteMessage : BaseMessage
    {
        public bool IsSuccess { get; init; }
    }
}
