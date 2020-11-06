namespace Typin
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Internal.Exceptions;

    /// <summary>
    /// Provides an implmentation of <see cref="ICliModeSwitcher"/> methods to switch and get current mode.
    /// </summary>
    internal sealed class CliModeSwitcher : ICliModeSwitcher
    {
        private readonly object _lock = new object();

        private Type? _currentModeType = null;
        private Type? _pendingModeType = null;

        private readonly IServiceProvider _serviceProvider;
        private readonly ApplicationConfiguration _configuration;

        /// <inheritdoc/>
        public ICliMode? Current { get; private set; }

        /// <inheritdoc/>
        public ICliMode? Pending { get; private set; }

        /// <inheritdoc/>
        public bool IsPending => !(Pending is null);

        /// <summary>
        /// Initializes an instance of <see cref="CliModeSwitcher"/>.
        /// </summary>
        public CliModeSwitcher(IServiceProvider serviceProvider, ApplicationConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;

            if (!ResetMode())
            {
                throw ModeSwitchingExceptions.InvalidStartupModeType(_configuration.StartupMode);
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
            lock (_lock)
            {
                if (_currentModeType != cliMode && (IsPending || _pendingModeType != cliMode))
                {
                    if (_serviceProvider.GetRequiredService(cliMode) is ICliMode cm)
                    {
                        _pendingModeType = cliMode;
                        Pending = cm;

                        return true;
                    }
                }
            }

            return false;
        }

        internal bool TrySwitchModes()
        {
            lock (_lock)
            {
                if (IsPending)
                {
                    _currentModeType = _pendingModeType;
                    _pendingModeType = null;

                    Current = Pending;
                    Pending = null;

                    return true;
                }
            }

            return false;
        }
    }
}
