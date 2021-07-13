namespace Typin.Hosting
{
    /// <summary>
    /// Provides an interface for implementing a factory that produces <see cref="IApplicationBuilder"/> instances.
    /// </summary>
    public interface IApplicationBuilderFactory
    {
        /// <summary>
        /// Create an <see cref="IApplicationBuilder" /> builder given a <paramref name="serverFeatures" />
        /// </summary>
        /// <returns>An <see cref="IApplicationBuilder"/> configured with <paramref name="serverFeatures"/>.</returns>
        IApplicationBuilder CreateBuilder();
    }
}