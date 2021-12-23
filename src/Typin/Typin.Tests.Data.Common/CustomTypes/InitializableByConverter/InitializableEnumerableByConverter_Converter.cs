namespace Typin.Tests.Data.Common.CustomTypes.InitializableByConverter
{
    using System.Collections.Generic;
    using System.Linq;
    using Typin.Models.Converters;

    public class InitializableEnumerableByConverter_Converter<T> : IArgumentConverter<InitializableEnumerableByConverter<T>>
    {
        public InitializableEnumerableByConverter<T>? Convert(string? value)
        {
            if (value is null)
            {
                return null;
            }

            T[] values = value.Split(':').Cast<T>().ToArray();

            return new InitializableEnumerableByConverter<T>(values);
        }

        public InitializableEnumerableByConverter<T> ConvertCollection(IReadOnlyCollection<string> values)
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