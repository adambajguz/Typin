namespace TypinExamples.Infrastructure.TypinWeb.Console.IO
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using TypinExamples.Application.Handlers.Commands.Terminal;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;

    public class WebTerminalWriter : Stream
    {
        private readonly IWorker _worker;

        private readonly StringBuilder _buffer = new StringBuilder();
        private readonly string _terminalId;

        /// <inheritdoc/>
        public override bool CanRead => false;

        /// <inheritdoc/>
        public override bool CanSeek => false;

        /// <inheritdoc/>
        public override bool CanWrite => true;

        /// <inheritdoc/>
        public override long Length => throw new NotSupportedException($"{nameof(WebTerminalReader)} does not support seeking.");

        /// <inheritdoc/>
        public override long Position
        {
            get => throw new NotSupportedException($"{nameof(WebTerminalReader)} does not support seeking.");
            set => throw new NotSupportedException($"{nameof(WebTerminalReader)} does not support seeking.");
        }

        public WebTerminalWriter(IWorker worker, string terminalId)
        {
            _worker = worker;
            _terminalId = terminalId;
        }

        /// <inheritdoc/>
        public override void Flush()
        {
            if (_buffer.Length == 0)
            {
                return;
            }

            string text = _buffer.ToString();
            _buffer.Clear();

            _worker.CallCommandAsync(new WriteCommand
            {
                TerminalId = _terminalId,
                Value = text
            }).Wait(10);
        }

        /// <inheritdoc/>
        public override async Task FlushAsync(CancellationToken cancellationToken)
        {
            if (_buffer.Length == 0)
            {
                return;
            }

            string text = _buffer.ToString();
            _buffer.Clear();

            await _worker.CallCommandAsync(new WriteCommand
            {
                TerminalId = _terminalId,
                Value = text
            });
        }

        /// <inheritdoc/>
        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException($"{nameof(WebTerminalReader)} does not support reading.");
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
            string text = Encoding.UTF8.GetString(buffer, offset, count);
            text = text.Replace(Environment.NewLine, "\r\n");

            _buffer.Append(text);
        }

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            string text = Encoding.UTF8.GetString(buffer, offset, count);
            text = text.Replace(Environment.NewLine, "\r\n");

            _buffer.Append(text);

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public override void Close()
        {
            base.Close();
            Flush();
        }
    }
}
