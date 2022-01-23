namespace Typin
{
    using System;
    using Typin.Features;

    /// <summary>
    /// Encapsulates all command call information.
    /// </summary>
    public abstract class CliContext
    {
        /// <summary>
        /// Features collection.
        /// </summary>
        public abstract IFeatureCollection Features { get; }

        /// <summary>
        /// Command line call informations feature.
        /// This feature should be initialized during <see cref="CliContext"/> instance creation.
        /// </summary>
        public abstract ICallInfoFeature Call { get; }

        /// <summary>
        /// Command line call services.
        /// This feature should be initialized during <see cref="CliContext"/> instance creation.
        /// </summary>
        public abstract IServiceProvider Services { get; }

        /// <summary>
        /// Command line call lifetime feature.
        /// This feature should be initialized during <see cref="CliContext"/> instance creation.
        /// </summary>
        public abstract ICallLifetimeFeature Lifetime { get; }

        /// <summary>
        /// Input feature.
        /// </summary>
        public abstract IInputFeature Input { get; }

        /// <summary>
        /// Tokenizer feature.
        /// This feature should be initialized inside pipeline.
        /// </summary>
        public abstract ITokenizerFeature Tokenizer { get; }

        /// <summary>
        /// Binder feature.
        /// This feature should be initialized inside pipeline.
        /// </summary>
        public abstract IBinderFeature Binder { get; }

        /// <summary>
        /// Output feature.
        /// This feature should be initialized during <see cref="CliContext"/> instance creation.
        /// </summary>
        public abstract IOutputFeature Output { get; }

        /// <summary>
        /// Initializes an instance of <see cref="CliContext"/>.
        /// </summary>
        public CliContext()
        {

        }
    }
}
