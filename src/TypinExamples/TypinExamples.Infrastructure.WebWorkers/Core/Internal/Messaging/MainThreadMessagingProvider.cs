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
        private event EventHandler<string> _callbacks = default!;
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
            _logger.LogDebug("{Class} -> {Method} {Message}", nameof(MainThreadMessagingProvider), nameof(OnMessage), rawMessage);

            _callbacks?.Invoke(this, rawMessage);
        }

        public async Task PostAsync(ulong? targetWorkerId, string rawMessage)
        {
            _logger.LogDebug("{Class} -> {Method} {Message}", nameof(MainThreadMessagingProvider), nameof(PostAsync), rawMessage);

            if (targetWorkerId is null)
                throw new ArgumentNullException(nameof(targetWorkerId));

            await _jsRuntime.InvokeVoidAsync($"{ScriptLoader.ModuleName}.postMessage", targetWorkerId, rawMessage);
        }

        public void Dispose()
        {
            _callbacks = default!;
        }
    }
}
