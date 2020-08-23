namespace Typin.Tests.Data.CustomTypes.Initializable
{
    using System;
    using Newtonsoft.Json;

    public class CustomStringParseableWithFormatProvider
    {
        public string Value { get; }

        [JsonConstructor]
        private CustomStringParseableWithFormatProvider(string value)
        {
            Value = value;
        }

        public static CustomStringParseableWithFormatProvider Parse(string value, IFormatProvider formatProvider)
        {
            return new CustomStringParseableWithFormatProvider(value + " " + formatProvider);
        }
    }
}