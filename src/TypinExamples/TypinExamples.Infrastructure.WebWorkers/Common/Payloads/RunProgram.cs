namespace TypinExamples.Infrastructure.WebWorkers.Common.Payloads
{
    public static class RunProgram
    {
        public class Payload
        {
            public string? ProgramClass { get; init; }
        }

        public class ResultPayload
        {
            public int ExitCode { get; init; }
        }
    }
}
