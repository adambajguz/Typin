﻿namespace TypinExamples.Infrastructure.WebWorkers.WorkerCore.Internal.JS
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    ///  Serves as a wrapper around a JSObject.
    /// </summary>
    internal sealed class DOMObject : IDisposable
    {
        public JSObject ManagedJSObject { get; private set; }

        public DOMObject(JSObject? jsobject)
        {
            ManagedJSObject = jsobject ?? throw new ArgumentNullException(nameof(jsobject));
        }

        public DOMObject(string globalName) : this(new JSObject(Runtime.GetGlobalObject(globalName)))
        {

        }

        public object Invoke(string method, params object[] args)
        {
            return ManagedJSObject.Invoke(method, args);
        }

        public async Task<object> InvokeAsync(string method, params object[] args)
        {
            return await Task.Run(() => ManagedJSObject.Invoke(method, args));
        }

        public void Dispose()
        {
            ManagedJSObject?.Dispose();
            ManagedJSObject = null!;
        }
    }
}
