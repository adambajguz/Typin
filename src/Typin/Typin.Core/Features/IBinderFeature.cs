namespace Typin.Features
{
    using Typin.Input;

    /// <summary>
    /// Binder feature.
    /// </summary>
    public interface IBinderFeature
    {
        /// <summary>
        /// Stores unbounded input data.
        /// </summary>
        UnboundedInput UnboundedInput { get; }
    }
}
