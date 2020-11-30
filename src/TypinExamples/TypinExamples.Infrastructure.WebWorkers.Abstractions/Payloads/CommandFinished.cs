namespace TypinExamples.Infrastructure.WebWorkers.Abstractions.Payloads
{
    using System.Threading.Tasks;

    public sealed class CommandFinished
    {
        public static ValueTask<CommandFinished> Task { get; } = ValueTask.FromResult(new CommandFinished());
    }
}
