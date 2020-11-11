namespace TypinExamples.TypinWeb
{
    using System;
    using System.IO;
    using System.Text;

    public class WebTerminalStream : Stream
    {
        private readonly IWebTerminal _webTerminal;

        public override bool CanRead { get; } = true;
        public override bool CanSeek { get; } = true;
        public override bool CanWrite { get; } = true;
        public override long Length { get; }
        public override long Position { get; set; }

        public WebTerminalStream(IWebTerminal webTerminal)
        {
            _webTerminal = webTerminal;
        }

        /// <inheritdoc/>
        public override void Flush()
        {

        }

        /// <inheritdoc/>
        public override int Read(byte[] buffer, int offset, int count)
        {
            return 'A';
        }

        /// <inheritdoc/>
        public override long Seek(long offset, SeekOrigin origin)
        {
            return 0;
        }

        /// <inheritdoc/>
        public override void SetLength(long value)
        {

        }

        /// <inheritdoc/>
        public override async void Write(byte[] buffer, int offset, int count)
        {
            string text = Encoding.UTF8.GetString(buffer, offset, count);

            await _webTerminal.WriteAsync(text.Replace(Environment.NewLine, "\r\n"));
        }
    }
}
