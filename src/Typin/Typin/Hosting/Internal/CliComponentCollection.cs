namespace Typin.Hosting.Components.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Hosting;
    using Typin.Hosting.Components;

    internal class CliComponentCollection : ICliComponentCollection
    {
        private readonly Dictionary<Type, CliComponent> _cliComponents = new();

        /// <inheritdoc/>
        public ICliBuilder CliBuilder { get; }

        public CliComponentCollection(ICliBuilder cliBuilder)
        {
            CliBuilder = cliBuilder;
        }

        /// <inheritdoc/>
        public TComponent GetOrAdd<TComponent>(Func<IServiceCollection, TComponent> factory)
            where TComponent : CliComponent
        {
            if (_cliComponents.TryGetValue(typeof(TComponent), out CliComponent? block))
            {
                return (block as TComponent)!;
            }

            TComponent b = factory(CliBuilder.Services);
            _cliComponents.Add(typeof(TComponent), b);

            return b;
        }

        /// <inheritdoc/>
        public TComponent GetOrAdd<TComponent>(Func<ICliBuilder, TComponent> factory)
            where TComponent : CliComponent
        {
            if (_cliComponents.TryGetValue(typeof(TComponent), out CliComponent? block))
            {
                return (block as TComponent)!;
            }

            TComponent b = factory(CliBuilder);
            _cliComponents.Add(typeof(TComponent), b);

            return b;
        }

        public CliComponentProvider Build()
        {
            return new CliComponentProvider(_cliComponents.ToDictionary(x => x.Key, x => x.Value.Types));
        }
    }
}
