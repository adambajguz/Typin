namespace Typin.Console.Internal
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Default console provider implementation.
    /// </summary>
    internal class ConsoleProvider : IConsoleProvider, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;

        private ConsoleProviderOptions _options;
        private readonly IDisposable _optionsMonitor;

        /// <inheritdoc/>
        public IEnumerable<string> Names => _options.Consoles.Names;

        /// <inheritdoc/>
        public IConsole? Default => this[string.Empty];

        /// <inheritdoc/>
        public IConsole? this[string name]
        {
            get
            {
                NamedConsoleDescriptor? descriptor = _options.Consoles[name];

                if (descriptor is null)
                {
                    return null;
                }

                return _serviceProvider.GetService(descriptor.ImplementationType) as IConsole;
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ConsoleProvider"/>.
        /// </summary>
        public ConsoleProvider(IServiceProvider serviceProvider,
                               IOptionsMonitor<ConsoleProviderOptions> options)
        {
            _serviceProvider = serviceProvider;

            _options = options.CurrentValue;
            _optionsMonitor = options.OnChange((value, namedOptions) =>
            {
                _options = value;
            });
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _optionsMonitor.Dispose();
        }
    }
}
