namespace Typin.Console
{
    using System;
    using System.IO;
    using Typin.Console.IO;

    /// <summary>
    /// ANSI/VT100 compatible console.
    /// </summary>
    public abstract partial class BaseConsole : IConsole
    {
        private ConsoleFeatures _enabledFeatures;

        /// <inheritdoc/>
        public abstract StandardStreamReader Input { get; }

        /// <inheritdoc/>
        public abstract StandardStreamWriter Output { get; }

        /// <inheritdoc/>
        public abstract StandardStreamWriter Error { get; }

        /// <inheritdoc/>
        public virtual ConsoleFeatures SupportedFeatures { get; } = ConsoleFeatures.None;

        /// <inheritdoc/>
        public ConsoleFeatures EnabledFeatures
        {
            get => _enabledFeatures;
            set
            {
                if ((SupportedFeatures | value) != SupportedFeatures)
                {
                    throw new NotSupportedException(
                        $"Value '{value}' contains one or more features that are not supported by {typeof(BaseConsole)}. " +
                        $"Supported features are: {SupportedFeatures}");
                }

                _enabledFeatures = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="BaseConsole"/>.
        /// </summary>
        protected BaseConsole()
        {
            _enabledFeatures = SupportedFeatures;
        }

        /// <summary>
        /// Validates the passed feature.
        /// </summary>
        /// <param name="feature"></param>
        /// <exception cref="NotSupportedException">Throws when <paramref name="feature"/> is not supported.</exception>
        protected void ValidateFeature(ConsoleFeatures feature)
        {
            if (!this.IsSupported(feature))
            {
                throw new NotSupportedException($"{feature} feature is not supported by {GetType()}.");
            }
        }

        /// <summary>
        /// Validates the passed features.
        /// </summary>
        /// <param name="feature0"></param>
        /// <param name="feature1"></param>
        /// <exception cref="NotSupportedException">Throws when <paramref name="feature0"/> and <paramref name="feature1"/> are not supported.</exception>
        protected void ValidateFeature(ConsoleFeatures feature0, ConsoleFeatures feature1)
        {
            if (!this.IsSupported(feature0) && !this.IsSupported(feature0))
            {
                throw new NotSupportedException($"{feature0} feature and {feature1} feature are not supported by {GetType()}.");
            }
        }

        #region Helpers
        private static StandardStreamReader WrapInput(IConsole console, Stream? stream, bool isRedirected, IKeyReader? keyReader)
        {
            if (stream is null)
            {
                return StandardStreamReader.CreateNull(console);
            }

            return new StandardStreamReader(Stream.Synchronized(stream), Console.InputEncoding, false, isRedirected, console, keyReader);
        }

        private static StandardStreamWriter WrapOutput(IConsole console, Stream? stream, bool isRedirected)
        {
            if (stream is null)
            {
                return StandardStreamWriter.CreateNull(console);
            }

            return new StandardStreamWriter(Stream.Synchronized(stream), Console.OutputEncoding, isRedirected, console)
            {
                AutoFlush = true
            };
        }
        #endregion
    }
}
