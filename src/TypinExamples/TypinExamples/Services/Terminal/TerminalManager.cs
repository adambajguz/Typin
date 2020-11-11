namespace TypinExamples.Services.Terminal
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components.Web;
    using Microsoft.JSInterop;
    using TypinExamples.Shared.Components;

    public class TerminalManager
    {
        private static readonly Dictionary<string, XTerm> _terminals = new Dictionary<string, XTerm>();

        public static void RegisterTerminal(string id, XTerm terminal)
        {
            _terminals[id] = terminal;
        }

        public static void UnregisterTerminal(string id)
        {
            if (_terminals.ContainsKey(id))
                _terminals.Remove(id);
        }

        [JSInvokable("ExampleInit")]
        public static async Task OnExampleInit(string id, string input)
        {

            if (_terminals.TryGetValue(id, out XTerm? term))
            {
                await term.RunExample(input);
            }
        }

        [JSInvokable]
        public static async Task OnKey(string id, KeyboardEventArgs @event)
        {
            if (_terminals.ContainsKey(id))
                await _terminals[id]?.OnKey.InvokeAsync(@event);
        }

        [JSInvokable]
        public static async Task OnLineFeed(string id)
        {
            if (_terminals.ContainsKey(id))
                await _terminals[id]?.OnLineFeed.InvokeAsync(string.Empty);
        }
    }
}