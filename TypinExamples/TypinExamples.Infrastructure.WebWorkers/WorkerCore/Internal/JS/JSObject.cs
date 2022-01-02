namespace TypinExamples.Infrastructure.WebWorkers.WorkerCore.Internal.JS
{
    using System;
    using System.Reflection;

    internal sealed class JSObject : IDisposable
    {
        public delegate object InvokeDelegate(string method, params object[] parameters);
        public delegate void DisposeDelegate();
        private readonly InvokeDelegate _invokeMethodDelegate;
        private readonly DisposeDelegate _disposeMethodDelegate;

        public JSObject(object target)
        {
            Type type = target.GetType();

            MethodInfo invokeMethod = type.GetMethod(nameof(Invoke)) ??
                                      throw new NullReferenceException($"{nameof(JSObject)}.{nameof(invokeMethod)}: failed to initialize");

            MethodInfo disposeMethod = type.GetMethod(nameof(Dispose)) ??
                                       throw new NullReferenceException($"{nameof(JSObject)}.{nameof(disposeMethod)}: failed to initialize"); ;

            _invokeMethodDelegate = Delegate.CreateDelegate(typeof(InvokeDelegate), target, invokeMethod) as InvokeDelegate ??
                                    throw new NullReferenceException($"{nameof(JSObject)}.{nameof(_invokeMethodDelegate)}: failed to initialize");

            _disposeMethodDelegate = Delegate.CreateDelegate(typeof(DisposeDelegate), target, disposeMethod) as DisposeDelegate ??
                                     throw new NullReferenceException($"{nameof(JSObject)}.{nameof(_invokeMethodDelegate)}: failed to initialize");
        }

        public object Invoke(string method, params object[] parameters)
        {
            return _invokeMethodDelegate(method, parameters);
        }

        public void Dispose()
        {
            _disposeMethodDelegate();
        }
    }
}