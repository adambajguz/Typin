namespace Typin.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Modes;

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
                                                          ModeBehavior.Default,
                                                          cancellationToken);
            }
            else if (cliOptions.CommandLineArguments is not null)
            {
                return await commandExecutor.ExecuteAsync(cliOptions.CommandLineArguments,
                                                          startupInputOptions,
                                                          ModeBehavior.Default,
                                                          cancellationToken);
            }

            return await commandExecutor.ExecuteAsync(string.Empty,
                                                      startupInputOptions,
                                                      ModeBehavior.Default,
                                                      cancellationToken);
        }
    }
}
