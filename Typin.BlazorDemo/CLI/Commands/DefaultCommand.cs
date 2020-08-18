using System.Threading.Tasks;
using Typin.Attributes;
using Typin.BlazorDemo.CLI.Services;

namespace Typin.BlazorDemo.CLI.Commands
{
    [Command(Description = "Runs webhost in normal mode.")]
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
