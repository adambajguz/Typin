namespace TypinExamples.Infrastructure.WebWorkers.Core.Internal.Messaging
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.JSInterop;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;
    using TypinExamples.Infrastructure.WebWorkers.Core.Internal.JS;

    /// <summary>
    /// Simple static message service that runs in the main thread.
    /// </summary>
    internal sealed class MainThreadMessagingProvider : IMessagingProvider
    {
        private event EventHandler<string> _callbacks;
        private readonly IJSRuntime _jsRuntime;

        public event EventHandler<string> Callbacks
        {
            add => _callbacks += value;
            remove => _callbacks -= value;
        }

        public MainThreadMessagingProvider(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        [JSInvokable]
        public void OnMessage(string rawMessage)
        {
#if DEBUG
            Console.WriteLine($"{nameof(MainThreadMessagingProvider)}.{nameof(OnMessage)}({rawMessage})");
#endif

            _callbacks?.Invoke(this, rawMessage);
        }

        public async Task PostAsync(ulong? id, string rawMessage)
        {
#if DEBUG
            Console.WriteLine($"{nameof(MainThreadMessagingProvider)}.{nameof(PostAsync)}({id}, {rawMessage})");
#endif

            if (id is null)
                throw new ArgumentNullException(nameof(id));

            await _jsRuntime.InvokeVoidAsync($"{ScriptLoader.MODULE_NAME}.postMessage", id, rawMessage);
        }

        public void Dispose()
        {

        }
    }
}
