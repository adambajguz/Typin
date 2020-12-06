namespace Typin.Modes
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Typin.Internal;

    /// <summary>
    /// Direct CLI mode. If no mode was registered or none of the registered modes was marked as startup, <see cref="DirectMode"/> will be registered.
    /// </summary>
    public class DirectMode : ICliMode
    {
        private readonly ICliApplicationLifetime _applicationLifetime;

        /// <summary>
        /// Mode options.
        /// </summary>
        public DirectModeSettings Options { get; }

        /// <summary>
        /// Initializes an instance of <see cref="DirectMode"/>.
        /// </summary>
        public DirectMode(IOptions<DirectModeSettings> options, ICliApplicationLifetime applicationLifetime)
        {
            Options = options.Value;
            _applicationLifetime = applicationLifetime;
        }

        /// <inheritdoc/>
        public async ValueTask<int> Execute(IReadOnlyList<string> commandLineArguments, ICliCommandExecutor executor)
        {
            int exitCode = await executor.ExecuteCommandAsync(commandLineArguments);
            _applicationLifetime.RequestStop();

            return exitCode;
        }
    }
}
