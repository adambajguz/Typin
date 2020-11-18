namespace TypinExamples.Services.Terminal
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.JSInterop;
    using TypinExamples.TypinWeb.Console;

    public class TerminalManager
    {
        private static readonly Dictionary<string, IWebTerminal> _terminals = new Dictionary<string, IWebTerminal>();

        public static void RegisterTerminal(string id, IWebTerminal terminal)
        {
            _terminals.TryAdd(id, terminal);
        }

        public static void UnregisterTerminal(string id)
        {
            if (_terminals.ContainsKey(id))
                _terminals.Remove(id);
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