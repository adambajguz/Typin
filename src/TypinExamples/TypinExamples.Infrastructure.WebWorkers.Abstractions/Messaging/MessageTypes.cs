namespace TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging
{
    using System;

    [Flags]
    public enum MessageTypes
    {
        FromMain = 1,
        FromWorker = 2,
        Call = 4,
        CallCommand = Call | Command,
        CallNotification= Call | Notification,
        Command = 8,
        Notification = 16,
        Result = 32,
        Exception = 64
    }
}