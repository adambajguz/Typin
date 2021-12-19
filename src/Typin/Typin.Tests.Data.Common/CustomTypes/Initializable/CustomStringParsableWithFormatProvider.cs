namespace Typin.Tests.Data.Common.CustomTypes.Initializable
{
    using System;
    using Newtonsoft.Json;

    public class CustomStringParsableWithFormatProvider
    {
        public string Value { get; }

        [JsonConstructor]
        private CustomStringParsableWithFormatProvider(string value)
        {
            Value = value;
        }

        public static CustomStringParsableWithFormatProvider Parse(string value, IFormatProvider formatProvider)
        {
            return new CustomStringParsableWithFormatProvider(value + " " + formatProvider.GetType().Name);
        }
    }
}