namespace Typin.Tests.Data.Commands.Valid
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Console;

    [Command("cmd")]
    public class CancellableCommand : ICommand
    {
        public const string CompletionOutputText = "Finished";
        public const string CancellationOutputText = "Canceled";

        private readonly IConsole _console;

        public CancellableCommand(IConsole console)
        {
            _console = console;
        }

        public async ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(3),
                                 cancellationToken);

                _console.Output.WriteLine(CompletionOutputText);
            }
            catch (OperationCanceledException)
            {
                _console.Output.WriteLine(CancellationOutputText);
                throw;
            }
        }
    }
}