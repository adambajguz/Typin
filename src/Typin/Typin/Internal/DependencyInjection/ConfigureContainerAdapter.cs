namespace Typin.Internal.DependencyInjection
{
    using System;

    internal class ConfigureContainerAdapter<TContainerBuilder> : IConfigureContainerAdapter
    {
        private readonly Action<TContainerBuilder> _action;

        public ConfigureContainerAdapter(Action<TContainerBuilder> action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public void ConfigureContainer(object containerBuilder)
        {
            _action((TContainerBuilder)containerBuilder);
        }
    }
}
