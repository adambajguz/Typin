namespace TypinExamples.Infrastructure.WebWorkers.Hil
{
    public class InitWorkerComplete : BaseMessage
    {
        public InitWorkerComplete()
        {
            MessageType = nameof(InitWorkerComplete);
        }
    }
}
