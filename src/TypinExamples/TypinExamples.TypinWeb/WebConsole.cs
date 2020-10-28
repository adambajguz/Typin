namespace TypinExamples.TypinWeb
{
    using System;
    using System.IO;
    using System.Threading;
    using Typin.Console;

    public class WebConsole : IConsole
    {
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

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public CancellationToken GetCancellationToken()
        {
            throw new NotImplementedException();
        }

        public ConsoleKeyInfo ReadKey(bool intercept = false)
        {
            throw new NotImplementedException();
        }

        public void ResetColor()
        {
            throw new NotImplementedException();
        }

        public void SetCursorPosition(int left, int top)
        {
            throw new NotImplementedException();
        }
    }
}
