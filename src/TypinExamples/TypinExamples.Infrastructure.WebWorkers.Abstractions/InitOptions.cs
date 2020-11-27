namespace TypinExamples.Infrastructure.WebWorkers.Abstractions
{
    using System;

    /// <summary>
    /// Options for initializing the worker.
    /// </summary>
    public class WorkerInitOptions
    {
        /// <summary>
        /// Specifies a list of assembly files names (dlls) that should be loaded when initializing the worker.
        /// </summary>
        public string[] DependentAssemblyFilenames { get; init; } = Array.Empty<string>();

        /// <summary>
        /// Mono-wasm-annotated endpoint for sending messages to the worker. Experts only.
        /// </summary>
        public string? MessageEndpoint { get; init; }

        /// <summary>
        /// Mono-wasm-annotated endpoint for instanciating the worker. Experts only.
        /// </summary>
        public string? InitEndpoint { get; init; }

        /// <summary>
        /// Unique blazor identifier for handling callbacks. As referenced by JSInvokableAttribute. Experts only.
        /// </summary>
        public string? CallbackMethod { get; init; }

        /// <summary>
        /// Whether BlazorWebWorker.js script debugging is enabled.
        /// </summary>
        public bool Debug { get; init; } = false;
    }
}