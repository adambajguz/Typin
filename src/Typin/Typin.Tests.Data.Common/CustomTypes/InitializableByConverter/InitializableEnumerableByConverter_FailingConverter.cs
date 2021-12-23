namespace Typin.Tests.Data.Common.CustomTypes.InitializableByConverter
{
    using System.Collections.Generic;
    using Typin.Models.Converters;

    public class InitializableEnumerableByConverter_FailingConverter<T> : IArgumentConverter<InitializableEnumerableByConverter<T>>
    {
        public InitializableEnumerableByConverter<T>? Convert(string? value)
        {
            throw new System.NotImplementedException();
        }

        public InitializableEnumerableByConverter<T> ConvertCollection(IReadOnlyCollection<string> values)
        {
            throw new System.NotImplementedException();
        }
    }
}