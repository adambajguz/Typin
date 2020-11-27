﻿namespace BlazorWorker.Core.SimpleInstanceService
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BlazorWorker.WorkerCore.SimpleInstanceService;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class SimpleInstanceServiceProxy : ISimpleInstanceService
    {
        private readonly IWorker worker;
        private readonly Dictionary<long, TaskCompletionSource<DisposeResult>> disposeResultSourceByCallId =
            new Dictionary<long, TaskCompletionSource<DisposeResult>>();
        private readonly Dictionary<long, TaskCompletionSource<InitInstanceResult>> initInstanceResultByCallId =
            new Dictionary<long, TaskCompletionSource<InitInstanceResult>>();
        private TaskCompletionSource<InitServiceResult> initWorker;

        private long callIdSource;

        public bool IsInitialized { get; internal set; }

        public SimpleInstanceServiceProxy(IWorker worker)
        {
            this.worker = worker;
            this.worker.IncomingMessage += OnIncomingMessage;
        }

        public async Task InitializeAsync(WorkerInitOptions options = null)
        {
            if (!IsInitialized)
            {
                if (!worker.IsInitialized)
                {
                    initWorker = new TaskCompletionSource<InitServiceResult>();
                    await worker.InitAsync(options);
                    await initWorker.Task;
                }

                IsInitialized = true;
            }
        }

        private void OnIncomingMessage(object sender, string message)
        {
#if DEBUG
            Console.WriteLine($"{nameof(SimpleInstanceServiceProxy)}:{message}");
#endif
            if (DisposeResult.CanDeserialize(message))
            {
                var result = DisposeResult.Deserialize(message);
                if (disposeResultSourceByCallId.TryGetValue(result.CallId, out var taskCompletionSource))
                {
                    taskCompletionSource.SetResult(result);
                }
                return;
            }

            if (InitServiceResult.CanDeserialize(message))
            {
                initWorker.SetResult(InitServiceResult.Deserialize(message));
                return;
            }

            if (InitInstanceResult.CanDeserialize(message))
            {
                var result = InitInstanceResult.Deserialize(message);
                if (initInstanceResultByCallId.TryGetValue(result.CallId, out var taskCompletionSource))
                {
                    taskCompletionSource.SetResult(result);
                }
                return;
            }
        }

        public async Task<DisposeResult> DisposeInstance(DisposeInstanceRequest request)
        {
            request.CallId = ++callIdSource;
            var res = new TaskCompletionSource<DisposeResult>();
            disposeResultSourceByCallId.Add(request.CallId, res);
            await worker.PostMessageAsync(request.Serialize());
            return await res.Task;
        }

        public async Task<InitInstanceResult> InitInstance(InitInstanceRequest request)
        {
            request.CallId = ++callIdSource;
            var res = new TaskCompletionSource<InitInstanceResult>();
            initInstanceResultByCallId.Add(request.CallId, res);
            await worker.PostMessageAsync(request.Serialize());
            return await res.Task;
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}