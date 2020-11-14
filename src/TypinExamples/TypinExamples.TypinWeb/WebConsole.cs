namespace TypinExamples.TypinWeb
{
    using System;
    using System.IO;
    using System.Threading;
    using Typin.Console;

    public sealed class WebConsole : IConsole
    {
        private readonly IWebTerminal _webTerminal;
        private readonly CancellationToken _cancellationToken;

        private ConsoleColor foregroundColor = ConsoleColor.White;
        private ConsoleColor backgroundColor = ConsoleColor.Black;

        public StreamReader Input { get; }
        public bool IsInputRedirected => false;

        public StreamWriter Output { get; }
        public bool IsOutputRedirected => false;

        public StreamWriter Error { get; }
        public bool IsErrorRedirected => false;

        public ConsoleColor ForegroundColor
        {
            get => foregroundColor;

            //https://misc.flogisoft.com/bash/tip_colors_and_formatting
            set
            {
                foregroundColor = value < ConsoleColor.Black || value > ConsoleColor.White ? ConsoleColor.White : value;

                string unicode = value switch
                {
                    ConsoleColor.Black => "\u001b[30m",
                    ConsoleColor.DarkRed => "\u001b[31m",
                    ConsoleColor.DarkGreen => "\u001b[32m",
                    ConsoleColor.DarkYellow => "\u001b[33m",
                    ConsoleColor.DarkBlue => "\u001b[34m",
                    ConsoleColor.DarkMagenta => "\u001b[35m",
                    ConsoleColor.DarkCyan => "\u001b[36m",
                    ConsoleColor.Gray => "\u001b[37m",
                    ConsoleColor.DarkGray => "\u001b[90m",
                    ConsoleColor.Red => "\u001b[91m",
                    ConsoleColor.Green => "\u001b[92m",
                    ConsoleColor.Yellow => "\u001b[93m",
                    ConsoleColor.Blue => "\u001b[94m",
                    ConsoleColor.Magenta => "\u001b[95m",
                    ConsoleColor.Cyan => "\u001b[96m",
                    ConsoleColor.White => "\u001b[97m",
                    _ => "\u001b[39m"
                };

                Output.Write(unicode);
            }
        }
        public ConsoleColor BackgroundColor
        {
            get => backgroundColor;
            set
            {
                backgroundColor = value < ConsoleColor.Black || value > ConsoleColor.White ? ConsoleColor.Black : value;

                string unicode = value switch
                {
                    ConsoleColor.Black => "\u001b[40m",
                    ConsoleColor.DarkRed => "\u001b[41m",
                    ConsoleColor.DarkGreen => "\u001b[42m",
                    ConsoleColor.DarkYellow => "\u001b[33m",
                    ConsoleColor.DarkBlue => "\u001b[44m",
                    ConsoleColor.DarkMagenta => "\u001b[45m",
                    ConsoleColor.DarkCyan => "\u001b[46m",
                    ConsoleColor.Gray => "\u001b[47m",
                    ConsoleColor.DarkGray => "\u001b[100m",
                    ConsoleColor.Red => "\u001b[101m",
                    ConsoleColor.Green => "\u001b[102m",
                    ConsoleColor.Yellow => "\u001b[103m",
                    ConsoleColor.Blue => "\u001b[104m",
                    ConsoleColor.Magenta => "\u001b[105m",
                    ConsoleColor.Cyan => "\u001b[106m",
                    ConsoleColor.White => "\u001b[107m",
                    _ => "\u001b[49m"
                };

                Output.Write(unicode);
            }
        }

        public int CursorLeft
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public int CursorTop
        {
            get => throw new NotImplementedException();
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

        public WebConsole(IWebTerminal webTerminal, CancellationToken cancellationToken = default)
        {
            _cancellationToken = cancellationToken;
            _webTerminal = webTerminal;

            Input = new StreamReader(new WebTerminalReader(webTerminal));

            Output = new StreamWriter(new WebTerminalWriter(webTerminal))
            {
                AutoFlush = true
            };

            Error = new StreamWriter(new WebTerminalWriter(webTerminal))
            {
                AutoFlush = true
            };
        }

        public async void Clear()
        {
            await _webTerminal.ClearAsync();
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
            foregroundColor = ConsoleColor.White;
            backgroundColor = ConsoleColor.Black;
            Output.Write("\u001b[39m\u001b[49m");
        }

        public void SetCursorPosition(int left, int top)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Input.Dispose();
            Output.Dispose();
            Error.Dispose();
        }
    }
}
