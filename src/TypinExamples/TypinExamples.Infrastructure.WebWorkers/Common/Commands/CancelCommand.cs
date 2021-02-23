namespace TypinExamples.Infrastructure.WebWorkers.Common.Payloads
{
    using System;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;

    internal sealed class CancelCommand : ICommand
    {
        public TimeSpan Delay { get; init; } = TimeSpan.Zero;
    }
}
