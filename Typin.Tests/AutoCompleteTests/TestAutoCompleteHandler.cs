namespace Typin.Tests.AutoCompleteTests
{
    using Typin.AutoCompletion;

    internal class TestAutoCompleteHandler : IAutoCompleteHandler
    {
        public char[] Separators { get; set; } = new char[] { ' ', '.', '/', '\\', ':' };

        public string[] GetSuggestions(string text, int index)
        {
            return new string[] { "World", "Angel", "Love" };
        }
    }
}
