namespace Typin.Internal.DependencyInjection
{
    internal interface IConfigureContainerAdapter
    {
        void ConfigureContainer(object containerBuilder);
    }
}
