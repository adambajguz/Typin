namespace Typin.Tests.Commands.Valid
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class NonInitializableEnumerable<T> : IEnumerable<T>
    {
        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)Array.Empty<T>()).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}