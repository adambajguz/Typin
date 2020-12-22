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

        private bool f = false;
        /// <inheritdoc/>
        public override int Read(byte[] buffer, int offset, int count)
        {
            ValidateReadParameters(buffer, offset, count);

#if DEBUG
            //Console.WriteLine("Read");
#endif
            f = !f;

            if (f)
            {
                buffer[offset + 0] = (byte)'\r';
                buffer[offset + 1] = (byte)'\n';

                return 2;
            }

            return 0;
        }

        /// <inheritdoc/>
        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            ValidateReadParameters(buffer, offset, count);

            // If cancellation was requested, bail early
            if (cancellationToken.IsCancellationRequested)
                return Task.FromCanceled<int>(cancellationToken);

            try
            {
#if DEBUG
                // Console.WriteLine("ReadAsyncBuf");
#endif
                f = !f;

                if (f)
                {
                    buffer[offset + 0] = (byte)'\r';
                    buffer[offset + 1] = (byte)'\n';

                    return Task<int>.FromResult(2);
                }

                return Task<int>.FromResult(0);
            }
            catch (OperationCanceledException)
            {
                return Task.FromCanceled<int>(cancellationToken);
            }
            catch (Exception exception)
            {
                return Task.FromException<int>(exception);
            }
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

        private static void ValidateReadParameters(byte[] buffer, int offset, int count)
        {
            if (buffer is null)
                throw new ArgumentNullException(nameof(buffer), "Buffer cannot be null.");

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Non-negative number required.");

            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), "Non-negative number required.");

            if ((uint)count > buffer.Length - offset)
                throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
        }
    }
}
