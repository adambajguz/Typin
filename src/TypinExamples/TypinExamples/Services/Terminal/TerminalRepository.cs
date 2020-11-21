namespace TypinExamples.Services.Terminal
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.JSInterop;
    using TypinExamples.Application.Services;
    using TypinExamples.TypinWeb.Console;

    public class TerminalRepository : ITerminalRepository
    {
        private static readonly Dictionary<string, IWebTerminal> _terminals = new Dictionary<string, IWebTerminal>();

        public TerminalRepository()
        {

        }

        public void RegisterTerminal(IWebTerminal terminal)
        {
            _terminals.TryAdd(terminal.Id, terminal);
        }

        public void UnregisterTerminal(string id)
        {
            if (_terminals.ContainsKey(id))
                _terminals.Remove(id);
        }

        public void UnregisterTerminal(IWebTerminal terminal)
        {
            if (_terminals.ContainsKey(terminal.Id))
                _terminals.Remove(terminal.Id);
        }

        public IWebTerminal? GetOrDefault(string id)
        {
            _terminals.TryGetValue(id, out IWebTerminal? term);

            return term;
        }

        [JSInvokable("TerminalManager_ExampleInit")]
        public static async Task OnExampleInit(string id, string input)
        {
            if (_terminals.TryGetValue(id, out IWebTerminal? term))
            {
                await term.RunExample(input);
            }
        }
    }
}