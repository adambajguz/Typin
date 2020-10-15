namespace Typin.Tests.Data.Commands.Valid
{
    using System;
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Console;

    [Command("cmd")]
    public class CancellableCommand : ICommand
    {
        public const string CompletionOutputText = "Finished";
        public const string CancellationOutputText = "Canceled";

        public async ValueTask ExecuteAsync(IConsole console)
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(3),
                                 console.GetCancellationToken());

                console.Output.WriteLine(CompletionOutputText);
            }
            catch (OperationCanceledException)
            {
                console.Output.WriteLine(CancellationOutputText);
                throw;
            }
        }
    }
}