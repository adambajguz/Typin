namespace Typin.Tests.Data.Common.CustomTypes.InitializableByConverter
{
    using System;
    using System.Collections.Generic;
    using Typin.Models.Converters;

    public class InitializableClassTypeByConverter_FailingConverter : IArgumentConverter<InitializableClassTypeByConverter>
    {
        public InitializableClassTypeByConverter? Convert(string? value)
        {
            throw new NotImplementedException();
        }

        public InitializableClassTypeByConverter ConvertCollection(IReadOnlyCollection<string> values)
        {
            throw new NotImplementedException();
        }
    }
}