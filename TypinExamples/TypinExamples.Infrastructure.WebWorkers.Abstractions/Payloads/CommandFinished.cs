namespace TypinExamples.Infrastructure.WebWorkers.Abstractions.Payloads
{
    using System.Threading.Tasks;

    public sealed class CommandFinished
    {
        public static CommandFinished Instance { get; } = new();
        public static ValueTask<CommandFinished> Task { get; } = ValueTask.FromResult(Instance);

        private CommandFinished()
        {

        }
    }
}
