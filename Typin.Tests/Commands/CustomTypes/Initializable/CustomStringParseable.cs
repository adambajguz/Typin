namespace Typin.Tests.Commands.CustomTypes.Initializable
{
    using Newtonsoft.Json;

    public class CustomStringParseable
    {
        public string Value { get; }

        [JsonConstructor]
        private CustomStringParseable(string value)
        {
            Value = value;
        }

        public static CustomStringParseable Parse(string value)
        {
            return new CustomStringParseable(value);
        }
    }
}