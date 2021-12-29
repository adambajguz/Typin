using Typin.Commands;

namespace Typin
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// <see cref="ICommandExecutor"/> extensions.
    /// </summary>
    public static class CommandExecutorExtensions
    {
        /// <summary>
        /// Executed command from <see cref="CliOptions"/>.
        /// <see cref="CliOptions.CommandLine"/> has a higher priority than <see cref="CliOptions.CommandLineArguments"/>.
        /// </summary>
        /// <param name="commandExecutor"></param>
        /// <param name="cliOptions"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<int> ExecuteAsync(this ICommandExecutor commandExecutor, CliOptions cliOptions, CancellationToken cancellationToken = default)
        {
            InputOptions startupInputOptions = cliOptions.StartupInputOptions;

            if (cliOptions.CommandLine is not null)
            {
                return await commandExecutor.ExecuteAsync(cliOptions.CommandLine,
                                                          startupInputOptions,
                                                          CommandExecutionOptions.Default,
                                                          cancellationToken);
            }
            else if (cliOptions.CommandLineArguments is not null)
            {
                return await commandExecutor.ExecuteAsync(cliOptions.CommandLineArguments,
                                                          startupInputOptions,
                                                          CommandExecutionOptions.Default,
                                                          cancellationToken);
            }

            return await commandExecutor.ExecuteAsync(string.Empty,
                                                      startupInputOptions,
                                                      CommandExecutionOptions.Default,
                                                      cancellationToken);
        }
    }
}
