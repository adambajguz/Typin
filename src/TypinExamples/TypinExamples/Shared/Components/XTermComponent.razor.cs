namespace TypinExamples.Shared.Components
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.Options;
    using Microsoft.JSInterop;
    using TypinExamples.Application.Configuration;
    using TypinExamples.Application.Services;
    using TypinExamples.Application.Services.TypinWeb;
    using TypinExamples.Application.Worker;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;
    using TypinExamples.Services.Terminal;

    public sealed partial class XTermComponent : ComponentBase
    {
        public string Id { get; } = string.Concat("m-", Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture));

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> InputAttributes { get; init; } = new Dictionary<string, object>();

        [Parameter]
        public string? ExampleKey { get; init; }

        //[Parameter]
        //public TerminalOptions Options { get; init; } = new TerminalOptions();

        [Parameter]
        public IWebLoggerDestination? LoggerDestination { get; init; }

        [Inject] private IJSRuntime JSRuntime { get; init; } = default!;
        [Inject] private ITerminalRepository TerminalRepository { get; init; } = default!;
        [Inject] private IOptions<ExamplesSettings> Options { get; init; } = default!;
        [Inject] private IWorkerFactory WorkerFactory { get; init; } = default!;

        private IWorker? _worker { get; set; }
        private bool IsInitialized => TerminalRepository.Contains(Id) && _worker is not null;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _worker ??= await WorkerFactory.CreateAsync<TypinWorkerStartup>();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!TerminalRepository.Contains(Id) && _worker is not null)
            {
                ExampleDescriptor descriptor = Options.Value.Examples?.Where(x => x.Key == ExampleKey ||
                                                                                  (x.Name?.Contains(ExampleKey ?? string.Empty) ?? false))
                                                                      .FirstOrDefault() ?? ExampleDescriptor.CreateDynamic();

                IWebTerminal terminal = new WebTerminal(Id, descriptor, JSRuntime, _worker);
                await terminal.InitializeXtermAsync();

                TerminalRepository.RegisterTerminal(terminal);
                StateHasChanged();

                await _worker.RunAsync();
            }
        }

        public async ValueTask DisposeAsync()
        {
            TerminalRepository.UnregisterTerminal(Id);
            IWebTerminal? terminal = TerminalRepository.GetOrDefault(Id);

            if (_worker is not null)
                await _worker.DisposeAsync();

            if (terminal is not null)
                await terminal.DisposeXtermAsync();
        }
    }
}