namespace Typin.Extensions
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
            CommandExecutionOptions startupExecutionOptions = cliOptions.StartupExecutionOptions;

            if (cliOptions.CommandLine is not null)
            {
                return await commandExecutor.ExecuteAsync(cliOptions.CommandLine,
                                                          startupExecutionOptions,
                                                          cancellationToken);
            }
            else if (cliOptions.CommandLineArguments is not null)
            {
                return await commandExecutor.ExecuteAsync(cliOptions.CommandLineArguments,
                                                          startupExecutionOptions,
                                                          cancellationToken);
            }

            return await commandExecutor.ExecuteAsync(string.Empty,
                                                      startupExecutionOptions,
                                                      cancellationToken);
        }
    }
}
