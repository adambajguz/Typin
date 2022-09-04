namespace Typin.Console
{
    using System;
    using System.IO;
    using Typin.Console.IO;

    /// <summary>
    /// Implementation of <see cref="IConsole"/> that routes all data to preconfigured streams.
    /// Does not leak to system console in any way.
    /// Use this class as a substitute for system console when running tests.
    /// </summary>
    public partial class VirtualConsole : BaseConsole
    {
        /// <inheritdoc />
        public override StandardStreamReader Input { get; }

        /// <inheritdoc />
        public override StandardStreamWriter Output { get; }

        /// <inheritdoc />
        public override StandardStreamWriter Error { get; }

        /// <inheritdoc />
        public override ConsoleFeatures SupportedFeatures { get; } = ConsoleFeatures.None;

        #region ctor
        /// <summary>
        /// Initializes an instance of <see cref="VirtualConsole"/>.
        /// Use named parameters to specify the streams you want to override.
        /// </summary>
        public VirtualConsole(Stream? input = null, bool isInputRedirected = true, IKeyReader? keyReader = null,
                              Stream? output = null, bool isOutputRedirected = true,
                              Stream? error = null, bool isErrorRedirected = true) :
            base()
        {
            Input = WrapInput(this, input, isInputRedirected, keyReader);
            Output = WrapOutput(this, output, isOutputRedirected);
            Error = WrapOutput(this, error, isErrorRedirected);
        }

        /// <summary>
        /// Creates a <see cref="VirtualConsole"/> that uses in-memory output and error streams.
        /// Use the exposed streams to easily get the current output.
        /// </summary>
        public static (VirtualConsole console, MemoryStreamWriter output, MemoryStreamWriter error) CreateBuffered(bool isInputRedirected = true,
                                                                                                                   bool isOutputRedirected = true,
                                                                                                                   bool isErrorRedirected = true)
        {
            // Memory streams don't need to be disposed
            MemoryStreamWriter output = new(Console.OutputEncoding);
            MemoryStreamWriter error = new(Console.OutputEncoding);

            VirtualConsole console = new(input: null, isInputRedirected, null,
                                         output.Stream, isOutputRedirected,
                                         error.Stream, isErrorRedirected);

            return (console, output, error);
        }

        /// <summary>
        /// Creates a <see cref="VirtualConsole"/> that uses in-memory output and error streams.
        /// Use the exposed streams to easily get the current output.
        /// </summary>
        public static (VirtualConsole console, MemoryStreamReader input, MemoryStreamWriter output, MemoryStreamWriter error) CreateBufferedWithInput(bool isInputRedirected = true,
                                                                                                                                                      bool isOutputRedirected = true,
                                                                                                                                                      bool isErrorRedirected = true)
        {
            // Memory streams don't need to be disposed
            MemoryStreamReader input = new(Console.InputEncoding);
            MemoryStreamWriter output = new(Console.OutputEncoding);
            MemoryStreamWriter error = new(Console.OutputEncoding);

            VirtualConsole console = new(input.Stream, isInputRedirected, null,
                                         output.Stream, isOutputRedirected,
                                         error.Stream, isErrorRedirected);

            return (console, input, output, error);
        }
        #endregion

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