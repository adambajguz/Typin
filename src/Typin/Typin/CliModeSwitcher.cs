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
        private readonly ApplicationConfiguration _applicationConfiguration;

        /// <inheritdoc/>
        public ICliMode? Current { get; private set; }

        /// <inheritdoc/>
        public ICliMode? Pending { get; private set; }

        /// <inheritdoc/>
        public bool IsPending => Pending is null;

        /// <summary>
        /// Initializes an instance of <see cref="CliModeSwitcher"/>.
        /// </summary>
        internal CliModeSwitcher(IServiceProvider serviceProvider, ApplicationConfiguration applicationConfiguration)
        {
            _serviceProvider = serviceProvider;
            _applicationConfiguration = applicationConfiguration;
            Reset();
        }

        /// <inheritdoc/>
        public bool Reset()
        {
            return Switch(_applicationConfiguration.StartupMode);
        }

        /// <inheritdoc/>
        public bool Switch<T>()
            where T : ICliMode
        {
            return Switch(typeof(T));
        }

        /// <inheritdoc/>
        public bool Switch(Type cliMode)
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

        internal bool PerformSwitching()
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
