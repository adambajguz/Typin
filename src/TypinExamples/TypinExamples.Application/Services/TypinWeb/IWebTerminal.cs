namespace TypinExamples.Application.Services
{
    using System;
    using System.Threading.Tasks;

    public interface IWebTerminal : IAsyncDisposable
    {
        string Id { get; }
        bool IsDisposed { get; }

        Task ResetAsync();
        Task ClearAsync();
        Task FocusAsync();
        Task BlurAsync();
        Task<int> GetRowsCountAsync();
        Task<int> GetColumnsCountAsync();
        Task WriteAsync(string str);
        Task WriteLineAsync(string str);
        Task ScrollLinesAsync(int lines);
        Task ScrollPagesAsync(int pages);
        Task ScrollToBottomAsync();
        Task ScrollToTopAsync();
        Task ScrollToLineAsync(int lineNumber);

        Task RunExample(string args);
    }
}
