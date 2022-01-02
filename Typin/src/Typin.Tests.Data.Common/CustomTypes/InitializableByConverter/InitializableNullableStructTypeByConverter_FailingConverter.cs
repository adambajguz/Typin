namespace Typin.Tests.Data.Common.CustomTypes.InitializableByConverter
{
    using System;
    using System.Collections.Generic;
    using Typin.Models.Converters;

    public class InitializableNullableStructTypeByConverter_FailingConverter : IArgumentConverter<InitializableStructTypeByConverter?>
    {
        public InitializableStructTypeByConverter? Convert(string? value)
        {
            throw new NotImplementedException();
        }

        public InitializableStructTypeByConverter? ConvertCollection(IReadOnlyCollection<string> values)
        {
            throw new NotImplementedException();
        }
    }
}