namespace Typin.Modes.Interactive.AutoCompletion
{
    internal interface IAutoCompletionHandler
    {
        /// <summary>
        /// Characters to start completion from.
        /// </summary>
        char[] Separators { get; set; }

        /// <summary>
        /// Returns suggestion for input.
        /// </summary>
        /// <param name="text">Current text entered in the console.</param>
        /// <param name="position">Position of the terminal cursor within text line.</param>
        /// <returns></returns>
        string[] GetSuggestions(string text, int position);
    }
}
