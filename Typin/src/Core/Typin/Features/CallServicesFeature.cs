namespace Typin.Features
{
    using System;

    /// <summary>
    /// <see cref="ICallServicesFeature"/> implementation.
    /// </summary>
    public sealed class CallServicesFeature : ICallServicesFeature
    {
        /// <inheritdoc/>
        public IServiceProvider CallServices { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="CallServicesFeature"/>.
        /// </summary>
        public CallServicesFeature(IServiceProvider callServices)
        {
            CallServices = callServices;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return base.ToString() +
                " | " +
                $"{nameof(CallServices)} = {{{CallServices}}}";
        }
    }
}
