namespace Typin.Modes
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Direct CLI mode. If no mode was registered or none of the registered modes was marked as startup, <see cref="DirectMode"/> will be registered.
    /// </summary>
    public class DirectMode : ICliMode
    {
        private readonly CliOptions _cliOptions;

        /// <summary>
        /// Initializes an instance of <see cref="DirectMode"/>.
        /// </summary>
        public DirectMode(IOptions<CliOptions> cliOptions)
        {
            _cliOptions = cliOptions.Value;
        }

        /// <inheritdoc/>
        public async ValueTask<int> ExecuteAsync(ICliCommandExecutor executor, bool isStartupContext, CancellationToken cancellationToken)
        {
            if (isStartupContext)
            {
                return await executor.ExecuteCommandAsync(
                    _cliOptions.CommandLine ?? string.Empty,
                    _cliOptions.CommandLineStartsWithExecutableName,
                    cancellationToken);
            }

            return ExitCodes.Success;
        }
    }
}
