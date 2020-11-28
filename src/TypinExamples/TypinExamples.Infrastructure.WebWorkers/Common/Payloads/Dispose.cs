namespace TypinExamples.Infrastructure.WebWorkers.Common.Payloads
{
    public static class Dispose
    {
        public sealed class Payload
        {

        }

        public sealed class ResultPayload
        {
            public bool IsSuccess { get; init; }
        }
    }
}
