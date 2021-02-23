namespace Typin
{
    using System;
    using Microsoft.Extensions.Logging;
    using Typin.Internal.Exceptions;

    /// <summary>
    /// Provides an implmentation of <see cref="ICliApplicationLifetime"/> to manage application lifetime.
    /// </summary>
    internal sealed class CliApplicationLifetime : ICliApplicationLifetime
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ApplicationConfiguration _configuration;
        private readonly ILogger _logger;

        /// <inheritdoc/>
        public CliLifetimes State { get; internal set; }

        /// <inheritdoc/>
        public ICliMode? CurrentMode { get; private set; }

        /// <inheritdoc/>
        public Type? CurrentModeType { get; private set; } = null;

        /// <inheritdoc/>
        public ICliMode? RequestedMode { get; private set; }

        /// <inheritdoc/>
        public Type? RequestedModeType { get; private set; } = null;

        /// <inheritdoc/>
        public bool IsModeRequested => !(RequestedMode is null);

        /// <summary>
        /// Initializes an instance of <see cref="CliApplicationLifetime"/>.
        /// </summary>
        public CliApplicationLifetime(IServiceProvider serviceProvider, ApplicationConfiguration configuration, ILogger<CliApplicationLifetime> logger)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _logger = logger;
            State = CliLifetimes.Starting;
        }

        internal void Initialize()
        {
            Type startupMode = _configuration.StartupMode;
            _logger.LogDebug("Initializing startup mode '{StartupMode}'.", startupMode);

            if (_serviceProvider.GetService(startupMode) is ICliMode cm)
            {
                CurrentMode = cm;
                CurrentModeType = startupMode;
                State = CliLifetimes.Running;
            }
            else
            {
                _logger.LogError("Failed to initialize startup mode '{StartupMode}'.", startupMode);

                throw ModeEndUserExceptions.InvalidStartupModeType(startupMode);
            }
        }

        /// <inheritdoc/>
        public bool ResetMode()
        {
            return RequestMode(_configuration.StartupMode);
        }

        /// <inheritdoc/>
        public bool RequestMode<T>()
            where T : ICliMode
        {
            return RequestMode(typeof(T));
        }

        /// <inheritdoc/>
        public bool RequestMode(Type cliMode)
        {
            if (CurrentModeType != cliMode && (IsModeRequested || RequestedModeType != cliMode))
            {
                if (_serviceProvider.GetService(cliMode) is ICliMode cm)
                {
                    _logger.LogInformation("Mode requested '{RequestedMode}'.", cliMode);

                    RequestedModeType = cliMode;
                    RequestedMode = cm;

                    return true;
                }
            }

            return false;
        }

        internal bool TrySwitchModes()
        {
            if (IsModeRequested)
            {
                _logger.LogDebug("Switching mode '{CurrentMode}' to '{RequestedMode}'.", CurrentModeType, RequestedModeType);

                CurrentModeType = RequestedModeType;
                RequestedModeType = null;

                CurrentMode = RequestedMode;
                RequestedMode = null;

                if (State == CliLifetimes.StopRequested)
                {
                    State = CliLifetimes.Running;
                    _logger.LogInformation("Stop request aborted by mode switching.");
                }

                return true;
            }

            return false;
        }

        public void RequestStop()
        {
            _logger.LogDebug("Requested CLI stop.", CurrentModeType, RequestedModeType);

            State = CliLifetimes.StopRequested;
        }

        internal void TryStop()
        {
            if (State == CliLifetimes.StopRequested)
            {
                State = CliLifetimes.Stopped;
                _logger.LogInformation("Stopping CLI application...");
            }
        }
    }
}
