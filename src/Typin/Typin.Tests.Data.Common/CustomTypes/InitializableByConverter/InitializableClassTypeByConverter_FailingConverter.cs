namespace Typin.Tests.Data.Common.CustomTypes.InitializableByConverter
{
    using System;
    using System.Collections.Generic;
    using Typin.Binding;

    public class InitializableClassTypeByConverter_FailingConverter : BindingConverter<InitializableClassTypeByConverter>
    {
        public override InitializableClassTypeByConverter? Convert(string? value)
        {
            throw new NotImplementedException();
        }

        public override InitializableClassTypeByConverter ConvertCollection(IReadOnlyCollection<string> values)
        {
            throw new NotImplementedException();
        }
    }
}