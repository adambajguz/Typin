namespace Typin.Tests.Data
{
    using System;
    using Typin.AutoCompletion;

    public class TestAutoCompleteHandler : IAutoCompletionHandler
    {
        public char[] Separators { get; set; } = new[] { ' ', '.', '/', '\\', ':' };

        public string[] GetSuggestions(string text, int index)
        {
            if (text.StartsWith('X'))
            {
                return Array.Empty<string>();
            }

            return new string[] { "World", "Angel", "Hello" };
        }
    }
}
