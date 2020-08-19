namespace Typin.Tests
{
    using System;
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Console;

    public partial class CancellationSpecs
    {
        [Command("cancel")]
        private class CancellableCommand : ICommand
        {
            public async ValueTask ExecuteAsync(IConsole console)
            {
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(3), console.GetCancellationToken());
                    console.Output.WriteLine("Never printed");
                }
                catch (OperationCanceledException)
                {
                    console.Output.WriteLine("Cancellation requested");
                    throw;
                }
            }
        }
    }
}