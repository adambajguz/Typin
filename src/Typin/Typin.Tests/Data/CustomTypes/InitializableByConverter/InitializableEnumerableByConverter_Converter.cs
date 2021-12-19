namespace Typin.Tests.Data.CustomTypes.InitializableByConverter
{
    using System.Collections.Generic;
    using System.Linq;
    using Typin.Binding;

    public class InitializableEnumerableByConverter_Converter<T> : BindingConverter<InitializableEnumerableByConverter<T>>
    {
        public override InitializableEnumerableByConverter<T>? Convert(string? value)
        {
            if (value is null)
            {
                return null;
            }

            T[] values = value.Split(':').Cast<T>().ToArray();

            return new InitializableEnumerableByConverter<T>(values);
        }

        public override InitializableEnumerableByConverter<T> ConvertCollection(IReadOnlyCollection<string> values)
        {
            List<T> v = new();

            foreach (string value in values)
            {
                v.AddRange(value.Split(':').Cast<T>());
            }

            return new InitializableEnumerableByConverter<T>(v);
        }
    }
}