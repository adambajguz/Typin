namespace Typin.Features
{
    using System;

    /// <summary>
    /// Command line call services feature.
    /// </summary>
    public interface ICallServicesFeature
    {
        /// <summary>
        /// Call services.
        /// </summary>
        IServiceProvider CallServices { get; }
    }
}
