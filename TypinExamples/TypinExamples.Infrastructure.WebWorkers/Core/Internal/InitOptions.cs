﻿namespace TypinExamples.Infrastructure.WebWorkers.Core.Internal
{
    using System;

    /// <summary>
    /// Options for initializing the worker.
    /// </summary>
    internal class WorkerInitOptions
    {
        /// <summary>
        /// Specifies a list of assembly files names (dlls) that should be loaded when initializing the worker.
        /// </summary>
        public string[] DependentAssemblyFilenames { get; init; } = Array.Empty<string>();

        /// <summary>
        /// Unique blazor identifier for handling callbacks. As referenced by JSInvokableAttribute. Experts only.
        /// </summary>
        public string? CallbackMethod { get; init; }

        /// <summary>
        /// Mono-wasm-annotated endpoint for sending messages to the worker. Experts only.
        /// </summary>
        public string? MessageEndpoint { get; init; }

        /// <summary>
        /// Mono-wasm-annotated endpoint for instanciating the worker. Experts only.
        /// </summary>
        public string? InitEndpoint { get; init; }

        /// <summary>
        /// Initialize message id.
        /// </summary>
        public ulong InitCallId { get; init; }

        /// <summary>
        /// Mono-wasm-annotated endpoint for instanciating the worker. Experts only.
        /// </summary>
        public string? StartupType { get; init; }

        /// <summary>
        /// Whether BlazorWebWorker.js script debugging is enabled.
        /// </summary>
        public bool Debug { get; init; } = false;
    }
}