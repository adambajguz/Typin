using System;
using System.Collections.Generic;

namespace Typin.Hosting.Components.Internal
{
    public interface ICliComponentProvider
    {
        IReadOnlyList<Type> Get(Type componentType);
        IReadOnlyList<Type> Get<T>();
    }
}