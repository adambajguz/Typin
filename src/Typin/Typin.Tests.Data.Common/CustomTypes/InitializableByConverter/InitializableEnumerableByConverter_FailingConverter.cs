namespace Typin.Tests.Data.CustomTypes.InitializableByConverter
{
    using System.Collections.Generic;
    using Typin.Binding;

    public class InitializableEnumerableByConverter_FailingConverter<T> : BindingConverter<InitializableEnumerableByConverter<T>>
    {
        public override InitializableEnumerableByConverter<T>? Convert(string? value)
        {
            throw new System.NotImplementedException();
        }

        public override InitializableEnumerableByConverter<T> ConvertCollection(IReadOnlyCollection<string> values)
        {
            throw new System.NotImplementedException();
        }
    }
}