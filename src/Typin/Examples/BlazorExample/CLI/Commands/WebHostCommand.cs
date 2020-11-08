namespace BlazorExample.CLI.Commands
{
    using System.Threading.Tasks;
    using BlazorExample.CLI.Services;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Modes;

    [Command("webhost", Description = "Management of the background webhost in the interactive mode.",
             SupportedModes = new[] { typeof(InteractiveMode) })]
    public class WebHostCommand : ICommand
    {
        private readonly IWebHostRunnerService _webHostRunnerService;

        public WebHostCommand(IWebHostRunnerService webHostRunnerService)
        {
            _webHostRunnerService = webHostRunnerService;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await _webHostRunnerService.RunAsync(console.GetCancellationToken());
        }
    }
}