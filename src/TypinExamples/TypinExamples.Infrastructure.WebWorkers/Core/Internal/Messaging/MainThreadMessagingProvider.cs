namespace TypinExamples.Infrastructure.WebWorkers.Core.Internal.Messaging
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
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
        private readonly ILogger _logger;

        public event EventHandler<string> Callbacks
        {
            add => _callbacks += value;
            remove => _callbacks -= value;
        }

        public MainThreadMessagingProvider(IJSRuntime jsRuntime, ILogger<MainThreadMessagingProvider> logger)
        {
            _jsRuntime = jsRuntime;
            _logger = logger;
        }

        [JSInvokable]
        public void OnMessage(string rawMessage)
        {
            _logger.LogDebug("{Class}->{Method} {Message}", nameof(MainThreadMessagingProvider), nameof(OnMessage), rawMessage);

            _callbacks?.Invoke(this, rawMessage);
        }

        public async Task PostAsync(ulong? workerId, string rawMessage)
        {
            _logger.LogDebug("{Class}->{Method} {Message}", nameof(MainThreadMessagingProvider), nameof(PostAsync), rawMessage);

            if (workerId is null)
                throw new ArgumentNullException(nameof(workerId));

            await _jsRuntime.InvokeVoidAsync($"{ScriptLoader.MODULE_NAME}.postMessage", workerId, rawMessage);
        }

        public void Dispose()
        {

        }
    }
}
