namespace TypinExamples.Infrastructure.WebWorkers.Common.Payloads
{
    public static class Call
    {
        public sealed class Payload
        {
            public string? ProgramClass { get; init; }
        }

        public sealed class ResultPayload
        {
            public string? Data { get; init; }
        }
    }
}
