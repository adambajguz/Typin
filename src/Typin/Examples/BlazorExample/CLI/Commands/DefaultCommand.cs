namespace BlazorExample.CLI.Commands
{
    using System.Threading.Tasks;
    using BlazorExample.CLI.Services;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command(Description = "Runs webhost in direct mode.")]
    public class DefaultCommand : ICommand
    {
        private readonly IWebHostRunnerService _webHostRunnerService;

        public DefaultCommand(IWebHostRunnerService webHostRunnerService)
        {
            _webHostRunnerService = webHostRunnerService;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await _webHostRunnerService.RunAsync(console.GetCancellationToken());
        }
    }
}
