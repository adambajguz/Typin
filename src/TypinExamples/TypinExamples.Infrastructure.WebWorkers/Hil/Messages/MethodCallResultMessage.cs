namespace TypinExamples.Infrastructure.WebWorkers.Hil
{
    using TypinExamples.Infrastructure.WebWorkers.Hil.Messages.Base;

    public class MethodCallResultMessage : BaseMessage
    {
        public int ExitCode { get; init; }
    }
}
