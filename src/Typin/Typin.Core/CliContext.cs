namespace Typin
{
    using Typin.Features;

    /// <summary>
    /// Encapsulates all CLI-specific information about a command.
    /// </summary>
    public abstract class CliContext
    {
        /// <summary>
        /// Features collection.
        /// </summary>
        public abstract IFeatureCollection Features { get; }

        /// <summary>
        /// Command line call informations feature.
        /// </summary>
        public abstract ICallInfoFeature Call { get; }

        /// <summary>
        /// Command line call lifetime feature.
        /// </summary>
        public abstract ICallLifetimeFeature Lifetime { get; }

        /// <summary>
        /// Command line mode feature.
        /// </summary>
        public abstract ICliModeFeature CliMode { get; }

        /// <summary>
        /// Input feature.
        /// </summary>
        public abstract IInputFeature Input { get; }

        /// <summary>
        /// Output feature.
        /// </summary>
        public abstract IOutputFeature Output { get; }

        /// <summary>
        /// Binder feature.
        /// </summary>
        public abstract IBinderFeature Binder { get; }

        /// <summary>
        /// Initializes an instance of <see cref="CliContext"/>.
        /// </summary>
        public CliContext()
        {

        }
    }
}
