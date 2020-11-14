namespace Typin
{
    using System;
    using Typin.Internal.Exceptions;

    /// <summary>
    /// Provides an implmentation of <see cref="ICliApplicationLifetime"/> to manage application lifetime.
    /// </summary>
    internal sealed class CliApplicationLifetime : ICliApplicationLifetime
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ApplicationConfiguration _configuration;

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
        public CliApplicationLifetime(IServiceProvider serviceProvider, ApplicationConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            State = CliLifetimes.Starting;
        }

        internal void Initialize()
        {
            Type startupMode = _configuration.StartupMode;
            if (_serviceProvider.GetService(startupMode) is ICliMode cm)
            {
                CurrentMode = cm;
                CurrentModeType = startupMode;
                State = CliLifetimes.Running;
            }
            else
            {
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
                CurrentModeType = RequestedModeType;
                RequestedModeType = null;

                CurrentMode = RequestedMode;
                RequestedMode = null;

                if (State == CliLifetimes.StopRequested)
                    State = CliLifetimes.Running;

                return true;
            }

            return false;
        }

        public void RequestStop()
        {
            State = CliLifetimes.StopRequested;
        }

        internal void TryStop()
        {
            if (State == CliLifetimes.StopRequested)
                State = CliLifetimes.Stopped;
        }
    }
}
