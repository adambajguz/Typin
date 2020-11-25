namespace BlazorWorker.Core
{
    using System;
    using System.Linq;

    internal class DependencyHintAttribute : Attribute
    {
        public DependencyHintAttribute(Type dependsOn, params Type[] dependsOnList)
        {
            DependsOn = new[] { dependsOn }.Concat(dependsOnList).ToArray();
        }

        public Type[] DependsOn { get; }
    }
}