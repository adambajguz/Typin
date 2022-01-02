namespace Typin.Tests.Data.Common.CustomTypes.InitializableByConverter
{
    using System.Collections;
    using System.Collections.Generic;

    public abstract class InitializableEnumerableByConverter : IEnumerable
    {
        public abstract IEnumerator GetEnumerator();
    }

    public class InitializableEnumerableByConverter<T> : InitializableEnumerableByConverter, IEnumerable<T>
    {
        private readonly IEnumerable<T> _arr;

        public InitializableEnumerableByConverter(IEnumerable<T> value)
        {
            _arr = value;
        }

        public override IEnumerator<T> GetEnumerator()
        {
            return _arr.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}