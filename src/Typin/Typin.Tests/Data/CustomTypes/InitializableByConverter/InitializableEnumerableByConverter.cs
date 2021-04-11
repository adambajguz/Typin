namespace Typin.Tests.Data.CustomTypes.InitializableByConverter
{
    using System.Collections;
    using System.Collections.Generic;

    public abstract class InitializableEnumerableByConverter : IEnumerable
    {
        public abstract IEnumerator GetEnumerator();
    }

    public class InitializableEnumerableByConverter<T> : InitializableEnumerableByConverter, IEnumerable<T>
    {
        private readonly T[] _arr;

        public InitializableEnumerableByConverter(T[] value)
        {
            _arr = value;
        }

        public override IEnumerator<T> GetEnumerator()
        {
            return (IEnumerator<T>)_arr.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}