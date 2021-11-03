namespace Typin.Modes
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Typin.Extensions;

    /// <summary>
    /// Direct CLI mode.
    /// </summary>
    public sealed class DirectMode : ICliMode
    {
        private readonly IOptionsMonitor<CliOptions> _cliOptions;
        private readonly ICliContextAccessor _cliContextAccessor;
        private readonly ICommandExecutor _commandExecutor;

        /// <summary>
        /// Initializes an instance of <see cref="DirectMode"/>.
        /// </summary>
        public DirectMode(IOptionsMonitor<CliOptions> cliOptions,
                          ICliContextAccessor cliContextAccessor,
                          ICommandExecutor commandExecutor)
        {
            _cliOptions = cliOptions;
            _cliContextAccessor = cliContextAccessor;
            _commandExecutor = commandExecutor;
        }

        /// <inheritdoc/>
        public async ValueTask<int> ExecuteAsync(CancellationToken cancellationToken)
        {
            if (_cliContextAccessor.CliContext.IsStartupContext())
            {
                CliOptions cliOptions = _cliOptions.CurrentValue;

                return await _commandExecutor.ExecuteAsync(
                    cliOptions.CommandLine ?? string.Empty,
                    cliOptions.StartupExecutionOptions,
                    cancellationToken);
            }

            return ExitCode.Success;
        }
    }
}
