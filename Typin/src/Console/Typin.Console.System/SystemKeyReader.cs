namespace Typin.Console
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Console.Extensions;
    using Typin.Console.IO;

    /// <summary>
    /// Key reader that uses system console.
    /// </summary>
    public sealed class SystemKeyReader : IKeyReader
    {
        private readonly TextReader _input;
        private readonly TextWriter _output;
        private readonly bool _isInputRedirected;

        /// <summary>
        /// Initializes a new instance of <see cref="SystemKeyReader"/>.
        /// </summary>
        public SystemKeyReader()
        {
            _input = Console.In;
            _output = Console.Out;
            _isInputRedirected = Console.IsInputRedirected;
        }

        /// <inheritdoc/>
        public ConsoleKeyInfo ReadKey(bool intercept = false)
        {
            //TODO: fix double enter for \r\
            if (_isInputRedirected)
            {
                int v = -1;
                while (v < 0)
                {
                    v = _input.Read();
                }

                _output.Write((char)v);

                return ((char)v).ToConsoleKeyInfo();
            }

            return Console.ReadKey(intercept);
        }

        /// <inheritdoc/>
        public async Task<ConsoleKeyInfo> ReadKeyAsync(bool intercept = false, CancellationToken cancellationToken = default)
        {
            char[] charsRead = new char[1];

            //TODO: fix double enter for \r\
            if (_isInputRedirected)
            {
                int v = -1;
                while (v < 0 && !cancellationToken.IsCancellationRequested)
                {
                    v = await _input.ReadAsync(charsRead, cancellationToken);
                }

                if (cancellationToken.IsCancellationRequested)
                {
                    throw new TaskCanceledException($"{nameof(ReadKeyAsync)} canceled.");
                }

                _output.Write(charsRead[0]);

                return charsRead[0].ToConsoleKeyInfo();
            }

            while (!Console.KeyAvailable && !cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1, cancellationToken);
            }

            if (cancellationToken.IsCancellationRequested)
            {
                throw new TaskCanceledException($"{nameof(ReadKeyAsync)} canceled.");
            }

            return Console.ReadKey(intercept);
        }
    }
}
