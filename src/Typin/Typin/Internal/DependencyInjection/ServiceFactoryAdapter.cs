namespace Typin.Internal.DependencyInjection
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    internal class ServiceFactoryAdapter<TContainerBuilder> : IServiceFactoryAdapter
    {
        private readonly IServiceProviderFactory<TContainerBuilder> _serviceProviderFactory;

        public ServiceFactoryAdapter(IServiceProviderFactory<TContainerBuilder> serviceProviderFactory)
        {
            _serviceProviderFactory = serviceProviderFactory ?? throw new ArgumentNullException(nameof(serviceProviderFactory));
        }

        public object CreateBuilder(IServiceCollection services)
        {
            return _serviceProviderFactory.CreateBuilder(services)!;
        }

        public IServiceProvider? CreateServiceProvider(object containerBuilder)
        {
            return _serviceProviderFactory.CreateServiceProvider((TContainerBuilder)containerBuilder);
        }
    }
}
