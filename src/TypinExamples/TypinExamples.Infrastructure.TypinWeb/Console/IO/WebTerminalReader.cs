namespace TypinExamples.Infrastructure.TypinWeb.Console.IO
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;

    public class WebTerminalReader : Stream
    {
        private readonly IWorker _worker;

        private readonly StringBuilder _buffer = new StringBuilder();
        private readonly string _terminalId;

        /// <inheritdoc/>
        public override bool CanRead => true;

        /// <inheritdoc/>
        public override bool CanSeek => false;

        /// <inheritdoc/>
        public override bool CanWrite => false;

        /// <inheritdoc/>
        public override long Length => throw new NotSupportedException($"{nameof(WebTerminalReader)} does not support seeking.");

        /// <inheritdoc/>
        public override long Position
        {
            get => throw new NotSupportedException($"{nameof(WebTerminalReader)} does not support seeking.");
            set => throw new NotSupportedException($"{nameof(WebTerminalReader)} does not support seeking.");
        }

        public WebTerminalReader(IWorker worker, string terminalId)
        {
            _worker = worker;
            _terminalId = terminalId;
        }

        /// <inheritdoc/>
        public override void Flush()
        {
            throw new IOException($"{nameof(WebTerminalReader)} can flush.");
        }

        /// <inheritdoc/>
        public override Task FlushAsync(CancellationToken cancellationToken)
        {
            throw new IOException($"{nameof(WebTerminalReader)} can flush.");
        }

        /// <inheritdoc/>
        public override int Read(byte[] buffer, int offset, int count)
        {
#if DEBUG
            Console.WriteLine("Read");
#endif
            buffer[0] = (byte)'\r';
            buffer[1] = (byte)'\n';

            return 0;
        }

        /// <inheritdoc/>
        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
#if DEBUG
            Console.WriteLine("ReadAsyncBuf");
#endif
            buffer[0] = (byte)'\r';
            buffer[1] = (byte)'\n';

            return Task<int>.FromResult(0);
        }

        /// <inheritdoc/>
        public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
#if DEBUG
            Console.WriteLine("ReadAsyncMem");
#endif
            return base.ReadAsync(buffer, cancellationToken);
        }

        /// <inheritdoc/>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException($"{nameof(WebTerminalReader)} does not support seeking.");
        }

        /// <inheritdoc/>
        public override void SetLength(long value)
        {
            throw new NotSupportedException($"{nameof(WebTerminalReader)} does not support seeking.");
        }

        /// <inheritdoc/>
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException($"{nameof(WebTerminalReader)} does not support writing.");
        }

        /// <inheritdoc/>
        public override void Close()
        {
            base.Close();
        }
    }
}
