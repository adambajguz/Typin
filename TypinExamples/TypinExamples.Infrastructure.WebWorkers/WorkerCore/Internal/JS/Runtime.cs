namespace TypinExamples.Infrastructure.WebWorkers.WorkerCore.Internal.JS
{
    using System;
    using System.Reflection;

    internal static class Runtime
    {
        private delegate object GetGlobalObjectDelegate(string globalObjectName);

        private static readonly GetGlobalObjectDelegate _getGlobalObjectMethod =
                Assembly.Load("System.Private.Runtime.InteropServices.JavaScript")
                        ?.GetType($"System.Runtime.InteropServices.JavaScript.{nameof(Runtime)}")
                        ?.GetMethod(nameof(GetGlobalObject))
                        ?.CreateDelegate(typeof(GetGlobalObjectDelegate)) as GetGlobalObjectDelegate ??
                throw new NullReferenceException($"{nameof(Runtime)}.{nameof(_getGlobalObjectMethod)}: failed to initialize");

        public static object GetGlobalObject(string globalObjectName)
        {
            return _getGlobalObjectMethod(globalObjectName);
        }
    }
}