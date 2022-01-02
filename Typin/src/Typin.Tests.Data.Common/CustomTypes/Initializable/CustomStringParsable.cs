namespace Typin.Tests.Data.Common.CustomTypes.Initializable
{
    using Newtonsoft.Json;

    public class CustomStringParsable
    {
        public string Value { get; }

        [JsonConstructor]
        private CustomStringParsable(string value)
        {
            Value = value;
        }

        public static CustomStringParsable Parse(string value)
        {
            return new CustomStringParsable(value);
        }
    }
}