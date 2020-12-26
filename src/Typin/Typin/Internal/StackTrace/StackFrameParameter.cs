namespace Typin.Internal.StackTrace
{
    internal class StackFrameParameter
    {
        public string Type { get; }

        public string? Name { get; }

        public StackFrameParameter(string type, string? name)
        {
            Type = type;
            Name = name;
        }
    }
}
