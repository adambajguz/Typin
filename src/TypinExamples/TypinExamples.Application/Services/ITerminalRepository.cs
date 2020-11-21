namespace TypinExamples.Application.Services
{
    using TypinExamples.TypinWeb.Console;

    public interface ITerminalRepository
    {
        IWebTerminal? GetOrDefault(string id);
        void RegisterTerminal(IWebTerminal terminal);
        void UnregisterTerminal(IWebTerminal terminal);
        void UnregisterTerminal(string id);
    }
}