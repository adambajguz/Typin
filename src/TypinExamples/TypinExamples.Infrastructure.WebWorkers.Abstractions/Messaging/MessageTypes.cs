namespace TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging
{
    using System;

    [Flags]
    public enum MessageTypes
    {
        FromMain = 1,
        FromWorker = 2,
        Call = 4,
        Result = 8,
        Exception = 16,
    }
}