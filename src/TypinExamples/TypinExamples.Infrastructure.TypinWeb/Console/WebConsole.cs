namespace TypinExamples.Infrastructure.TypinWeb.Console
{
    using System;
    using System.Threading;
    using Typin.Console;
    using Typin.Console.IO;
    using TypinExamples.Application.Handlers.Commands.Terminal;
    using TypinExamples.Application.Services;
    using TypinExamples.Infrastructure.TypinWeb.Console.IO;

    public sealed class WebConsole : IConsole
    {
        private readonly TimerService _flushTimer = new TimerService();

        private readonly ICoreMessageDispatcher _coreMessageDispatcher;
        private readonly string _terminalId;

        private readonly CancellationToken _cancellationToken;

        private ConsoleColor _foregroundColor = ConsoleColor.White;
        private ConsoleColor _backgroundColor = ConsoleColor.Black;

        private int _cursorLeft;
        private int _cursorTop;

        public StandardStreamReader Input { get; }
        public StandardStreamWriter Output { get; }
        public StandardStreamWriter Error { get; }

        public ConsoleColor ForegroundColor
        {
            get => _foregroundColor;

            //https://misc.flogisoft.com/bash/tip_colors_and_formatting
            set
            {
                _foregroundColor = value < ConsoleColor.Black || value > ConsoleColor.White ? ConsoleColor.White : value;

                Output.Write(Ansi.Color.Foreground.FromConsoleColor(value));
            }
        }
        public ConsoleColor BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                _backgroundColor = value < ConsoleColor.Black || value > ConsoleColor.White ? ConsoleColor.Black : value;

                Output.Write(Ansi.Color.Background.FromConsoleColor(value));
            }
        }

        public int CursorLeft
        {
            get => _cursorLeft;
            set => throw new NotImplementedException();
        }

        public int CursorTop
        {
            get => _cursorTop;
            set => throw new NotImplementedException();
        }

        public int WindowWidth
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public int WindowHeight
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public int BufferWidth
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public int BufferHeight
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public WebConsole(ICoreMessageDispatcher coreMessageDispatcher, string terminalId, CancellationToken cancellationToken = default)
        {
            _coreMessageDispatcher = coreMessageDispatcher;
            _terminalId = terminalId;
            _cancellationToken = cancellationToken;

            Input = new StandardStreamReader(new WebTerminalReader(coreMessageDispatcher, terminalId), false, this);

            Output = new StandardStreamWriter(new WebTerminalWriter(coreMessageDispatcher, terminalId), false, this)
            {
                AutoFlush = false
            };

            Error = new StandardStreamWriter(new WebTerminalWriter(coreMessageDispatcher, terminalId), false, this)
            {
                AutoFlush = false
            };

            _flushTimer.Elapsed += FlushTimerElapsed;
            _flushTimer.Set(50, true);
        }

        private void FlushTimerElapsed()
        {
            Console.WriteLine("Flush");
            Output.FlushAsync().Wait(25);
            Error.FlushAsync().Wait(25);
        }

        public void Clear()
        {
            WorkerMessage message = WorkerMessageBuilder<WorkerMessageFromWorkerBuilder>.CreateFromWorker()
                                .CallCommand(new ClearCommand
                                {
                                    TerminalId = _terminalId,
                                })
                                .Build();

            _coreMessageDispatcher.DispatchAsync(message).Wait(10);
        }

        public CancellationToken GetCancellationToken()
        {
            return _cancellationToken;
        }

        public ConsoleKeyInfo ReadKey(bool intercept = false)
        {
            throw new NotImplementedException();
        }

        public void ResetColor()
        {
            _foregroundColor = ConsoleColor.White;
            _backgroundColor = ConsoleColor.Black;

            Output.Write(string.Concat(Ansi.Color.Foreground.Default, Ansi.Color.Background.Default));
        }

        public void SetCursorPosition(int left, int top)
        {
            _cursorLeft = left;
            _cursorTop = top;

            Output.Write(Ansi.Cursor.Move.ToLocation(left, top));
        }

        public void Dispose()
        {
            _flushTimer.Dispose();

            Input.Dispose();
            Output.Dispose();
            Error.Dispose();
        }
    }
}
