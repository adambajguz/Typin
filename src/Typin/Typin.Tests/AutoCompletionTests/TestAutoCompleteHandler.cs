namespace Typin.Tests.AutoCompletionTests
{
    using Typin.AutoCompletion;

    internal class TestAutoCompleteHandler : IAutoCompletionHandler
    {
        public char[] Separators { get; set; } = new char[] { ' ', '.', '/', '\\', ':' };

        public string[] GetSuggestions(string text, int index)
        {
            return new string[] { "World", "Angel", "Hello" };
        }
    }
}
