namespace BlazorExample.CLI.Commands
{
    using System.Threading.Tasks;
    using BlazorExample.CLI.Services;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Exceptions;
    using Typin.Modes;

    [Command("webhost restart", Description = "Restarts the webhost worker in the interactive mode.",
             SupportedModes = new[] { typeof(InteractiveMode) })]
    public class WebHostRestartCommand : ICommand
    {
        private readonly IBackgroundWebHostProvider _webHostProvider;

        public WebHostRestartCommand(IBackgroundWebHostProvider webHostProvider)
        {
            _webHostProvider = webHostProvider;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            if (_webHostProvider.Status == WebHostStatuses.Stopped)
                throw new CommandException("WebHost is stopped.");

            await _webHostProvider.RestartAsync(console.GetCancellationToken());
        }
    }
}
