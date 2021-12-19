namespace Typin.Tests.Data.CustomTypes.InitializableByConverter
{
    using System;
    using System.Collections.Generic;
    using Typin.Binding;

    public class InitializableClassTypeByConverter_Converter : BindingConverter<InitializableClassTypeByConverter>
    {
        public override InitializableClassTypeByConverter? Convert(string? value)
        {
            if (value is null)
            {
                return null;
            }

            string[] values = value.Split(':');

            if (values.Length != 2)
            {
                throw new FormatException($"Invalid format of {nameof(InitializableClassTypeByConverter)}.");
            }

            DayOfWeek day = Enum.Parse<DayOfWeek>(values[0]);
            int v = int.Parse(values[1]);

            return new InitializableClassTypeByConverter { Value = v, Day = day };
        }

        public override InitializableClassTypeByConverter ConvertCollection(IReadOnlyCollection<string> values)
        {
            throw new NotImplementedException();
        }
    }
}