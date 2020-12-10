namespace Typin.Modes
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Direct CLI mode. If no mode was registered or none of the registered modes was marked as startup, <see cref="DirectMode"/> will be registered.
    /// </summary>
    public class DirectMode : ICliMode
    {
        private readonly ICliApplicationLifetime _applicationLifetime;

        /// <summary>
        /// Initializes an instance of <see cref="DirectMode"/>.
        /// </summary>
        public DirectMode(ICliApplicationLifetime applicationLifetime)
        {
            _applicationLifetime = applicationLifetime;
        }

        /// <inheritdoc/>
        public async ValueTask<int> ExecuteAsync(IReadOnlyList<string> commandLineArguments, ICliCommandExecutor executor)
        {
            int exitCode = await executor.ExecuteCommandAsync(commandLineArguments);
            _applicationLifetime.RequestStop();

            return exitCode;
        }
    }
}
