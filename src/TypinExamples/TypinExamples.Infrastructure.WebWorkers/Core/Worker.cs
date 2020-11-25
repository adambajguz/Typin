namespace TypinExamples.Infrastructure.WebWorkers.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.JSInterop;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;
    using TypinExamples.Infrastructure.WebWorkers.Hil;
    using TypinExamples.Infrastructure.WebWorkers.WorkerCore;

    public class Worker<T> : IWorker
        where T : class, IWebWorkerEntryPoint
    {
        private readonly string[] _assemblies;

        private static long idSource;

        private readonly IJSRuntime _jsRuntime;
        private readonly ScriptLoader _scriptLoader;
        private bool isDisposed = false;
        private static readonly string messageMethod;

        public event EventHandler<string> IncomingMessage;
        public bool IsInitialized { get; private set; }
        static Worker()
        {
            var messageServiceType = typeof(MessageService);
            messageMethod = $"[{messageServiceType.Assembly.GetName().Name}]{messageServiceType.FullName}:{nameof(MessageService.OnMessage)}";
        }

        public Worker(IJSRuntime jsRuntime, string[] assemblies)
        {
            _assemblies = assemblies;
            _jsRuntime = jsRuntime;
            _scriptLoader = new ScriptLoader(_jsRuntime);
            Identifier = ++idSource;
        }

        public async ValueTask DisposeAsync()
        {
            if (!isDisposed)
            {
                await _jsRuntime.InvokeVoidAsync("BlazorWorker.disposeWorker", Identifier);
                isDisposed = true;
            }
        }

        public async Task<int> RunAsync()
        {
            // Create service reference. For most scenarios, it's safe (and best) to keep this
            // reference around somewhere to avoid the startup cost.
            var service = await CreateBackgroundServiceAsync();

            // Reference that live outside of the current scope should not be passed into the expression.
            // To circumvent this, create a scope-local variable like this, and pass the local variable.
            int exitCode = await service.RunAsync();

            return exitCode;
        }

        private async Task<IWorkerBackgroundService<T>> CreateBackgroundServiceAsync()
        {
            var proxy = new WorkerBackgroundServiceProxy<T>(this, new WebWorkerOptions());

            await proxy.InitAsync();
            return proxy;
        }

        public async Task InitAsync(string initEndpoint)
        {
            await _scriptLoader.InitScript();

            await _jsRuntime.InvokeVoidAsync(
                "BlazorWorker.initWorker",
                Identifier,
                DotNetObjectReference.Create(this),
                new WorkerInitOptions
                {
                    DependentAssemblyFilenames = _assemblies,
                    CallbackMethod = nameof(OnMessage),
                    MessageEndPoint = messageMethod,
                    InitEndPoint = initEndpoint
                });
        }

        [JSInvokable]
        public async Task OnMessage(string message)
        {
            IncomingMessage?.Invoke(this, message);
        }

        public async Task PostMessageAsync(string message)
        {
            await _jsRuntime.InvokeVoidAsync("BlazorWorker.postMessage", Identifier, message);
        }

        public long Identifier { get; }
    }
}
