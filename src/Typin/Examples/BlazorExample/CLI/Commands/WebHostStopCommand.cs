namespace BlazorExample.CLI.Commands
{
    using System.Threading.Tasks;
    using BlazorExample.CLI.Services;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Modes;

    [Command("webhost stop", Description = "Stops the webhost background worker in the interactive mode.",
             SupportedModes = new[] { typeof(InteractiveMode) })]
    public class WebHostStopCommand : ICommand
    {
        private readonly IBackgroundWebHostProvider _webHostProvider;

        public WebHostStopCommand(IBackgroundWebHostProvider webHostProvider)
        {
            _webHostProvider = webHostProvider;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await _webHostProvider.StopAsync(console.GetCancellationToken());
        }
    }
}
