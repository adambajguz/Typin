namespace TypinExamples.Infrastructure.WebWorkers.Common.Payloads
{
    using System;

    internal sealed class CancelPayload
    {
        public TimeSpan Delay { get; init; } = TimeSpan.Zero;
    }
}
