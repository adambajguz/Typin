namespace Typin
{
    using System;
    using Typin.Features;

    /// <summary>
    /// Encapsulates all CLI-specific information about a command.
    /// </summary>
    public sealed class DefaultCliContext : CliContext
    {
        /// <inheritdoc/>
        public override IFeatureCollection Features { get; } = new FeatureCollection();

        /// <inheritdoc/>
        public override ICallInfoFeature Call => Features.Get<ICallInfoFeature>() ??
            throw new InvalidOperationException("Call has not been configured for this application or call.");

        /// <inheritdoc/>
        public override ICallLifetimeFeature Lifetime => Features.Get<ICallLifetimeFeature>() ??
            throw new InvalidOperationException("Lifetime has not been configured for this application or call.");

        /// <inheritdoc/>
        public override ICliModeFeature CliMode => Features.Get<ICliModeFeature>() ??
            throw new InvalidOperationException("CliMode has not been configured for this application or call.");

        /// <inheritdoc/>
        public override IInputFeature Input => Features.Get<IInputFeature>() ??
            throw new InvalidOperationException("Input has not been configured for this application or call.");

        /// <inheritdoc/>
        public override IOutputFeature Output => Features.Get<IOutputFeature>() ??
            throw new InvalidOperationException("Output has not been configured for this application or call.");

        /// <inheritdoc/>
        public override IBinderFeature Binder => Features.Get<IBinderFeature>() ??
            throw new InvalidOperationException("Binder has not been configured for this application or call.");

        /// <summary>
        /// Initializes an instance of <see cref="CliContext"/>.
        /// </summary>
        public DefaultCliContext() :
            base()
        {

        }
    }
}
