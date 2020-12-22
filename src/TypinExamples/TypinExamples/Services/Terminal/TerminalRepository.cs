namespace TypinExamples.Services.Terminal
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Microsoft.JSInterop;
    using TypinExamples.Application.Configurations;
    using TypinExamples.Application.Services;
    using TypinExamples.Application.Services.TypinWeb;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;

    public sealed class TerminalRepository : ITerminalRepository
    {
        private static readonly Dictionary<string, IWebTerminal> _terminals = new Dictionary<string, IWebTerminal>();

        private IJSRuntime _runtime { get; }
        private IOptions<ExamplesConfiguration> _options { get; }

        public TerminalRepository(IJSRuntime runtime, IOptions<ExamplesConfiguration> options)
        {
            _runtime = runtime;
            _options = options;
        }

        public async Task<IWebTerminal> CreateTerminalAsync(string id, string exampleKey, IWorker worker)
        {
            ExampleDescriptor descriptor = _options.Value.Descriptors.Where(x => x.Key == exampleKey || (x.Name?.Contains(exampleKey ?? string.Empty) ?? false))
                                                                  .First();

            IWebTerminal terminal = new WebTerminal(id, descriptor, _runtime, worker);
            await terminal.InitializeXtermAsync();

            _terminals.TryAdd(id, terminal);

            return terminal;
        }

        public async Task UnregisterAndDisposeTerminalAsync(string id)
        {
            if (GetOrDefault(id) is IWebTerminal terminal)
            {
                _terminals.Remove(id);
                await terminal.DisposeXtermAsync();
            }
        }

        public bool Contains(string id)
        {
            return _terminals.ContainsKey(id);
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

        public void Dispose()
        {
            foreach (IWebTerminal terminal in _terminals.Values)
            {
                terminal.DisposeXtermAsync().Wait(100);
            }

            _terminals.Clear();
        }
    }
}