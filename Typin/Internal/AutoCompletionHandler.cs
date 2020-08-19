namespace Typin.Internal
{
    using System;
    using Typin.AutoCompletion;

    internal class AutoCompletionHandler : IAutoCompleteHandler
    {
        // characters to start completion from
        public char[] Separators { get; set; } = new char[] { ' ', '.', '/' };

        // text - The current text entered in the console
        // index - The index of the terminal cursor within {text}
        public string[] GetSuggestions(string text, int index)
        {
            if (text.StartsWith("git "))
                return new string[] { "init", "clone", "pull", "push" };
            else if (text.StartsWith("test "))
                return new string[] { "aa", "bb", "cc", "dd" };
            else
                return Array.Empty<string>();
        }
    }
}
