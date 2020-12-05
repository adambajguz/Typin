namespace TypinExamples.Shared.Components
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.Logging;
    using TypinExamples.Application.Services.TypinWeb;
    using TypinExamples.Application.Worker;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;

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

        [Inject] private ITerminalRepository TerminalRepository { get; init; } = default!;
        [Inject] private IWorkerFactory WorkerFactory { get; init; } = default!;
        [Inject] private ILogger<XTermComponent> Logger { get; init; } = default!;

        private IWorker? _worker { get; set; }
        private bool IsInitialized => TerminalRepository.Contains(Id) && _worker is not null;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _worker ??= await WorkerFactory.CreateAsync<TypinWorkerStartup>();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!TerminalRepository.Contains(Id) && _worker is IWorker worker)
            {
                await TerminalRepository.CreateTerminalAsync(Id, ExampleKey ?? string.Empty, worker);
                StateHasChanged();

                await _worker.RunAsync();
            }
        }

        public async Task TerminateTerminal()
        {
            if (TerminalRepository.Contains(Id) && _worker is IWorker worker)
            {
                //Kill the old worker
                const int miliseconds = 2000;
                TaskAwaiter awaiter = worker.CancelAsync().GetAwaiter();
                await Task.Delay(miliseconds);

                if (!awaiter.IsCompleted || !worker.IsCancelled)
                    Logger.LogWarning("Worker {Id} does not respond, thus failed to cancel within {Time} miliseconds.", worker.Id, miliseconds);

                _worker = null;
                await TerminalRepository.UnregisterAndDisposeTerminalAsync(Id);
                StateHasChanged();

                await worker.DisposeAsync();

                //Create a new worker
                worker = await WorkerFactory.CreateAsync<TypinWorkerStartup>();

                await TerminalRepository.CreateTerminalAsync(Id, ExampleKey ?? string.Empty, worker);
                _worker = worker;
                StateHasChanged();

                try
                {
                    await worker.RunAsync();
                }
                catch (Exception) when (worker.IsDisposed)
                {
                    Logger.LogWarning("Execution of program was cancelled, and worker (Id) was disposed. No result was returned from program.", worker.Id);
                }
            }
        }

        public async ValueTask DisposeAsync()
        {
            await TerminalRepository.UnregisterAndDisposeTerminalAsync(Id);

            if (_worker is not null)
                await _worker.DisposeAsync();
        }
    }
}