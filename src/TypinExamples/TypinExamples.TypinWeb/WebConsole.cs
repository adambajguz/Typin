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

        public StreamReader Input { get; }
        public bool IsInputRedirected { get; }
        public StreamWriter Output { get; }
        public bool IsOutputRedirected { get; }
        public StreamWriter Error { get; }
        public bool IsErrorRedirected { get; }
        public ConsoleColor ForegroundColor { get; set; }
        public ConsoleColor BackgroundColor { get; set; }
        public int CursorLeft { get; set; }
        public int CursorTop { get; set; }
        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }
        public int BufferWidth { get; set; }
        public int BufferHeight { get; set; }

        public WebConsole(IWebTerminal webTerminal, CancellationToken cancellationToken = default)
        {
            _cancellationToken = cancellationToken;
            _webTerminal = webTerminal;

            Input = new StreamReader(new WebTerminalStream(webTerminal));

            Output = new StreamWriter(new WebTerminalStream(webTerminal))
            {
                AutoFlush = true
            };

            Error = new StreamWriter(new WebTerminalStream(webTerminal))
            {
                AutoFlush = true
            };
        }

        public async void Clear()
        {
            await _webTerminal.ClearAsync();
        }

        public void Dispose()
        {
            Input.Dispose();
            Output.Dispose();
            Error.Dispose();
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

        }

        public void SetCursorPosition(int left, int top)
        {

        }
    }
}
