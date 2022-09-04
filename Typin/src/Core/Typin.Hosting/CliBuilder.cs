namespace Typin.Hosting
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Hosting;
    using Typin.Hosting.Components;
    using Typin.Hosting.Scanning;

    /// <summary>
    /// Base CLI builder.
    /// </summary>
    public abstract class CliBuilder : ICliBuilder, IDisposable
    {
        private readonly IComponentProvider? _previousComponentProvider;
        private readonly Dictionary<Type, IScanner> _components = new();
        private bool disposedValue;

        /// <inheritdoc/>
        [MemberNotNullWhen(true, nameof(_previousComponentProvider))]
        public bool SubsequentCall { get; }

        /// <inheritdoc/>
        public HostBuilderContext Context { get; }

        /// <inheritdoc/>
        public IHostEnvironment Environment { get; }

        /// <inheritdoc/>
        public IConfiguration Configuration { get; }

        /// <inheritdoc/>
        public IServiceCollection Services { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="CliBuilder"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="services"></param>
        protected CliBuilder(HostBuilderContext context, IServiceCollection services)
        {
            Context = context;
            Environment = context.HostingEnvironment;
            Configuration = context.Configuration;
            Services = services;

            _previousComponentProvider = services.Where(x => x.ServiceType == typeof(IComponentProvider))
                .FirstOrDefault()
                ?.ImplementationInstance as IComponentProvider;

            Services.RemoveAll<IComponentProvider>();

            SubsequentCall = _previousComponentProvider is not null;
        }

        /// <inheritdoc/>
        public TInterface GetScanner<TComponent, TInterface>(Func<ICliBuilder, IReadOnlyCollection<Type>?, TInterface> factory)
            where TComponent : class
            where TInterface : class, IScanner<TComponent>
        {
            if (!_components.TryGetValue(typeof(TComponent), out IScanner? componentScanner))
            {
                IReadOnlyCollection<Type>? previous = _previousComponentProvider?.Get<TComponent>();

                componentScanner = factory(this, previous);
                _components.Add(typeof(TComponent), componentScanner);
            }

            return (componentScanner as TInterface)!;
        }

        /// <summary>
        /// Finalizes <see cref="CliBuilder"/>.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    DisposeInternal();
                }

                _components.Clear();
                disposedValue = true;
            }
        }

        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private void DisposeInternal()
        {
            var cliComponents = _components.ToDictionary(x => x.Key, x => x.Value.Types);

            if (SubsequentCall)
            {
                foreach (Type type in _previousComponentProvider.ComponentTypes)
                {
                    if (!cliComponents.ContainsKey(type))
                    {
                        cliComponents[type] = _previousComponentProvider.Get(type) ?? new HashSet<Type>();
                    }
                }
            }
            else
            {
                Services.AddSingleton(typeof(IComponents<>), typeof(Components<>));
            }

            ComponentProvider cliComponentProvider = new(cliComponents);
            Services.AddSingleton<IComponentProvider>(cliComponentProvider);
        }
    }
}
