namespace Typin.Internal
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Exceptions.Mode;
    using Typin.Schemas;

    internal sealed class CliModeManager : ICliModeSwitcher, ICliModeAccessor
    {
        private readonly AsyncLocal<CurrentModeHolder?> _currentCliMode = new();

        private readonly ConcurrentDictionary<Type, ICliMode> _cliModes = new();
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Gets or sets mode.
        /// </summary>
        /// <exception cref="InvalidModeException">Throws when type is not a valid mode type on set.</exception>

        public Type? Type
        {
            get => _currentCliMode.Value?.Type;
            private set
            {
                if (Type != value && value is not null)
                {
                    ICliMode instance = _cliModes.GetOrAdd(value, (t) =>
                    {
                        return _serviceProvider.GetService(t) as ICliMode ?? throw new InvalidModeException(t); //TODO: maybe cli mode should be scoped?
                    });

                    _currentCliMode.Value = new CurrentModeHolder(value, instance);
                }
            }
        }

        /// <inheritdoc/>
        public ICliMode? Instance => _currentCliMode.Value?.Instance;

        /// <summary>
        /// Initializes a new instance of <see cref="CliModeManager"/>.
        /// </summary>
        public CliModeManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc/>
        public async Task WithModeAsync<TMode>(Func<TMode, CancellationToken, Task> @delegate, CancellationToken cancellationToken)
            where TMode : class, ICliMode
        {
            await Task.Run(async () =>
            {
                Type = typeof(TMode);

                TMode instance = Instance as TMode ?? throw new NullReferenceException($"{nameof(ICliMode)} instance is null.");
                await @delegate(instance, cancellationToken);

                Type = null;

            }, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task WithModeAsync(Type modeType, Func<ICliMode, CancellationToken, Task> @delegate, CancellationToken cancellationToken)
        {
            _ = modeType ?? throw new ArgumentNullException(nameof(modeType));

            if (!KnownTypesHelpers.IsCliModeType(modeType))
            {
                throw new InvalidModeException(modeType);
            }

            await Task.Run(async () =>
            {
                Type = modeType;

                ICliMode instance = _currentCliMode.Value?.Instance ?? throw new NullReferenceException($"{nameof(ICliMode)} instance is null.");
                await @delegate(instance, cancellationToken);

                Type = null;

            }, cancellationToken);
        }

        private class CurrentModeHolder
        {
            public Type Type;
            public ICliMode Instance;

            public CurrentModeHolder(Type type, ICliMode instance)
            {
                Type = type;
                Instance = instance;
            }
        }
    }
}
