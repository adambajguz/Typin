namespace TypinExamples.Infrastructure.TypinWeb.Console.IO
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using TypinExamples.Application.Handlers.Commands.Terminal;
    using TypinExamples.Application.Services.Workers;
    using TypinExamples.Domain.Builders;
    using TypinExamples.Domain.Models.Workers;

    public class WebTerminalWriter : Stream
    {
        private readonly ICoreMessageDispatcher _coreMessageDispatcher;

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

        public WebTerminalWriter(ICoreMessageDispatcher coreMessageDispatcher, string terminalId)
        {
            _coreMessageDispatcher = coreMessageDispatcher;
            _terminalId = terminalId;
        }

        /// <inheritdoc/>
        public override void Flush()
        {
            string text = _buffer.ToString();
            _buffer.Clear();

            WorkerMessage message = WorkerMessageBuilder<WorkerMessageFromWorkerBuilder>.CreateFromWorker()
                                         .CallCommand(new WriteCommand
                                         {
                                             TerminalId = _terminalId,
                                             Value = text
                                         })
                                         .Build();

            _coreMessageDispatcher.DispatchAsync(message).Wait(10);
        }

        /// <inheritdoc/>
        public override async Task FlushAsync(CancellationToken cancellationToken)
        {
            string text = _buffer.ToString();
            _buffer.Clear();

            WorkerMessage message = WorkerMessageBuilder<WorkerMessageFromWorkerBuilder>.CreateFromWorker()
                             .CallCommand(new WriteCommand
                             {
                                 TerminalId = _terminalId,
                                 Value = text
                             })
                             .Build();

            await _coreMessageDispatcher.DispatchAsync(message);
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
