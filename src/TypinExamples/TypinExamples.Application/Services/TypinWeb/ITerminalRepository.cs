namespace TypinExamples.Application.Services.TypinWeb
{
    public interface ITerminalRepository
    {
        IWebTerminal? GetOrDefault(string id);
        void RegisterTerminal(IWebTerminal terminal);
        void UnregisterTerminal(IWebTerminal terminal);
        void UnregisterTerminal(string id);
    }
}