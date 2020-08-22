namespace Typin.Tests.Commands.CustomTypes.Initializable
{
    public class CustomStringConstructible
    {
        public string Value { get; }

        public CustomStringConstructible(string value)
        {
            Value = value;
        }
    }
}