namespace Typin.Hosting
{
    using System;
    using System.Collections.Generic;

    public class MiddlewarePipelineProvider
    {
        public LinkedList<Type> Middlewares { get; internal set; } = new();
    }
}