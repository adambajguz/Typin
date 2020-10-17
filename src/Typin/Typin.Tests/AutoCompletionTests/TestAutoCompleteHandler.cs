namespace Typin.Tests.AutoCompletionTests
{
    using System;
    using Typin.AutoCompletion;

    internal class TestAutoCompleteHandler : IAutoCompletionHandler
    {
        public char[] Separators { get; set; } = new char[] { ' ', '.', '/', '\\', ':' };

        public string[] GetSuggestions(string text, int index)
        {
            if (text.StartsWith('X'))
                return Array.Empty<string>();

            return new string[] { "World", "Angel", "Hello" };
        }
    }
}
