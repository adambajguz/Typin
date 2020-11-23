namespace TypinExamples.Infrastructure.TypinWeb.Console.IO
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using TypinExamples.Application.Services;

    public class WebTerminalWriter : Stream
    {
        private readonly StringBuilder _buffer = new StringBuilder();
        private readonly IWebTerminal _webTerminal;

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

        public WebTerminalWriter(IWebTerminal webTerminal)
        {
            _webTerminal = webTerminal;
        }

        /// <inheritdoc/>
        public override void Flush()
        {
            string text = _buffer.ToString();
            _buffer.Clear();

            _webTerminal.WriteAsync(text).Wait(10);
        }

        /// <inheritdoc/>
        public override async Task FlushAsync(CancellationToken cancellationToken)
        {
            string text = _buffer.ToString();
            _buffer.Clear();

            await _webTerminal.WriteAsync(text);
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

        /// <inheritdoc/>
        public override void Close()
        {
            base.Close();
            Flush();
        }
    }
}
