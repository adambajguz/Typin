namespace TypinExamples.Shared.Components
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using Blazored.Toast.Services;
    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.Logging;
    using TypinExamples.Application.Services.TypinWeb;
    using TypinExamples.Application.Worker;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;

    public sealed partial class XTermComponent : ComponentBase, IAsyncDisposable
    {
        public string Id { get; } = string.Concat("m-", Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture));

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> InputAttributes { get; init; } = new Dictionary<string, object>();

        [Parameter]
        public string? ExampleKey { get; init; }

        //[Parameter]
        //public TerminalOptions Options { get; init; } = new TerminalOptions();

        [Inject] private ITerminalRepository TerminalRepository { get; init; } = default!;
        [Inject] private IWorkerFactory WorkerFactory { get; init; } = default!;
        [Inject] private ILogger<XTermComponent> Logger { get; init; } = default!;
        [Inject] private IToastService ToastService { get; init; } = default!;

        private IWorker? WorkerInstance { get; set; }
        private bool IsInitialized => TerminalRepository.Contains(Id) && WorkerInstance is not null;
        private TaskCompletionSource WorkerInitSource { get; set; } = new();

        private Action<WorkerCreationConfiguration> WorkerCreationConfiguration { get; }

        public XTermComponent()
        {
            WorkerCreationConfiguration = (cfg) =>
            {
                cfg.ExcludedAssemblied = new string[]
                {
                    "TypinExamples.dll",

                    "Serilog.Sinks.BrowserConsole.dll",
                    "Blazored.Toast.dll",
                    "Markdig.dll",

                    "Microsoft.AspNetCore.Authorization.dll",
                    "Microsoft.AspNetCore.Components.dll",
                    "Microsoft.AspNetCore.Components.Forms.dll",
                    "Microsoft.AspNetCore.Components.Web.dll",
                    "Microsoft.AspNetCore.Components.WebAssembly.dll",
                    "Microsoft.AspNetCore.Metadata.dll",

                    "Microsoft.JSInterop.dll",
                    "Microsoft.JSInterop.WebAssembly.dll"
                };
            };
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            WorkerInstance ??= await WorkerFactory.CreateAsync<TypinWorkerStartup>(WorkerCreationConfiguration, onInitStarted: (id) => ToastService.ShowInfo($"Initializing worker ({id})..."));
            WorkerInitSource.SetResult();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (!TerminalRepository.Contains(Id) && WorkerInstance is IWorker worker)
            {
                await TerminalRepository.CreateTerminalAsync(Id, ExampleKey ?? string.Empty, worker);

                StateHasChanged();

                ToastService.ShowSuccess($"Terminal emulation ready on worker ({worker.Id}).");

                await WorkerInstance.RunAsync();
            }
        }

        public async Task TerminateTerminal()
        {
            if (TerminalRepository.Contains(Id) && WorkerInstance is IWorker worker)
            {
                WorkerInitSource = new();

                ToastService.ShowInfo("Terminal is about to shutdown...");

                //Kill the old worker
                const int miliseconds = 2000;
                TaskAwaiter awaiter = worker.CancelAsync().GetAwaiter();
                await Task.Delay(miliseconds);

                if (!awaiter.IsCompleted || !worker.IsCancelled)
                {
                    Logger.LogWarning("Worker {Id} does not respond, thus failed to cancel within {Time} miliseconds.", worker.Id, miliseconds);
                    ToastService.ShowWarning($"Worker {worker.Id} does not respond. Executing forced disposal...");
                }

                WorkerInstance = null;
                await TerminalRepository.UnregisterAndDisposeTerminalAsync(Id);
                StateHasChanged();

                await worker.DisposeAsync();

                ToastService.ShowSuccess($"Disposed worker ({worker.Id}).");

                //Create a new worker
                worker = await WorkerFactory.CreateAsync<TypinWorkerStartup>(WorkerCreationConfiguration, onInitStarted: (id) => ToastService.ShowInfo($"Initializing worker ({id})..."));

                await TerminalRepository.CreateTerminalAsync(Id, ExampleKey ?? string.Empty, worker);
                WorkerInstance = worker;
                StateHasChanged();

                WorkerInitSource.SetResult();

                ToastService.ShowSuccess($"Terminal emulation ready on worker ({worker.Id}).");

                try
                {
                    await worker.RunAsync();
                }
                catch (Exception) when (worker.IsDisposed)
                {
                    Logger.LogWarning("Execution of program was cancelled, and worker (Id) was disposed. No result was returned from program.", worker.Id);
                    ToastService.ShowWarning($"Execution of program was cancelled, and worker ({worker.Id}) was disposed. No result was returned from program.");
                }
            }
        }

        public async ValueTask DisposeAsync()
        {
            await WorkerInitSource.Task;

            ulong? id = null;
            if (WorkerInstance is not null)
            {
                id = WorkerInstance.Id;
                await WorkerInstance.DisposeAsync();
            }

            await TerminalRepository.UnregisterAndDisposeTerminalAsync(Id);

            Logger.LogInformation("Disposed XTermcomponenet {Id}", Id);
            ToastService.ShowSuccess($"Disposed worker ({id}).");
        }
    }
}