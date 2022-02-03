namespace Typin.Console
{
    using System;
    using System.IO;
    using Typin.Console.Internal;
    using Typin.Console.IO;

    /// <summary>
    /// Implementation of <see cref="IConsole"/> that wraps the default system console.
    /// </summary>
    public partial class SystemConsole : BaseConsole
    {
        /// <inheritdoc/>
        public override StandardStreamReader Input { get; }

        /// <inheritdoc/>
        public override StandardStreamWriter Output { get; }

        /// <inheritdoc/>
        public override StandardStreamWriter Error { get; }

        /// <inheritdoc/>
        public override ConsoleFeatures SupportedFeatures { get; } =
            ConsoleFeatures.ConsoleColors |
            ConsoleFeatures.Clear |
            ConsoleFeatures.CursorPosition |
            ConsoleFeatures.CursorVisibility |
            ConsoleFeatures.WindowSize |
            ConsoleFeatures.BufferSize;

        /// <summary>
        /// Initializes an instance of <see cref="SystemConsole"/>.
        /// </summary>
        public SystemConsole() :
            this(new SystemKeyReader())
        {

        }

        /// <summary>
        /// Initializes an instance of <see cref="SystemConsole"/>.
        /// </summary>
        protected SystemConsole(IKeyReader? keyReader) :
            base()
        {
            Input = WrapInput(this, Console.OpenStandardInput(), Console.IsInputRedirected, keyReader);
            Output = WrapOutput(this, Console.OpenStandardOutput(), Console.IsOutputRedirected);
            Error = WrapOutput(this, Console.OpenStandardError(), Console.IsErrorRedirected);
        }

        #region Helpers
        private static StandardStreamReader WrapInput(IConsole console, Stream? stream, bool isRedirected, IKeyReader? keyReader)
        {
            if (stream is null)
            {
                return StandardStreamReader.CreateNull(console);
            }

            return new StandardStreamReader(stream,
                                            Console.InputEncoding,
                                            false,
                                            8192,
                                            isRedirected,
                                            console,
                                            keyReader);
        }

        private static StandardStreamWriter WrapOutput(IConsole console, Stream? stream, bool isRedirected)
        {
            if (stream is null)
            {
                return StandardStreamWriter.CreateNull(console);
            }

            return new StandardStreamWriter(stream,
                                            Console.OutputEncoding.WithoutPreamble(),
                                            4096,
                                            isRedirected,
                                            console)
            {
                AutoFlush = true
            };
        }
        #endregion
    }
}