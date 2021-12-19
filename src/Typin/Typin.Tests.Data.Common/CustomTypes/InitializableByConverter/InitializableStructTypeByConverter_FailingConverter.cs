namespace Typin.Tests.Data.Common.CustomTypes.InitializableByConverter
{
    using System;
    using System.Collections.Generic;
    using Typin.Binding;

    public class InitializableStructTypeByConverter_FailingConverter : BindingConverter<InitializableStructTypeByConverter>
    {
        public override InitializableStructTypeByConverter Convert(string? value)
        {
            throw new NotImplementedException();
        }

        public override InitializableStructTypeByConverter ConvertCollection(IReadOnlyCollection<string> values)
        {
            throw new NotImplementedException();
        }
    }
}