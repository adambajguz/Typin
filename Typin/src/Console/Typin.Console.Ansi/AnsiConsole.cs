namespace Typin.Console
{
    using System;
    using System.IO;
    using Typin.Console.IO;

    /// <summary>
    /// ANSI/VT100 compatible console.
    /// </summary>
    public partial class AnsiConsole : BaseConsole
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
            ConsoleFeatures.RgbColors |
            ConsoleFeatures.Clear |
            ConsoleFeatures.CursorPosition |
            ConsoleFeatures.CursorVisibility;

        /// <summary>
        /// Initializes a new instance of <see cref="AnsiConsole"/>.
        /// The streams are not being disposed by default.
        /// </summary>
        public AnsiConsole(Stream? input = null, bool isInputRedirected = true, IKeyReader? keyReader = null,
                           Stream? output = null, bool isOutputRedirected = true,
                           Stream? error = null, bool isErrorRedirected = true)
        {
            Input = WrapInput(this, input, isInputRedirected, keyReader);
            Output = WrapOutput(this, output, isOutputRedirected);
            Error = WrapOutput(this, error, isErrorRedirected);
        }

        #region Helpers
        private static StandardStreamReader WrapInput(IConsole console, Stream? stream, bool isRedirected, IKeyReader? keyReader)
        {
            if (stream is null)
            {
                return StandardStreamReader.CreateNull(console);
            }

            return new StandardStreamReader(stream, Console.InputEncoding, false, isRedirected, console, keyReader);
        }

        private static StandardStreamWriter WrapOutput(IConsole console, Stream? stream, bool isRedirected)
        {
            if (stream is null)
            {
                return StandardStreamWriter.CreateNull(console);
            }

            return new StandardStreamWriter(stream, Console.OutputEncoding, isRedirected, console)
            {
                AutoFlush = true
            };
        }
        #endregion
    }
}
