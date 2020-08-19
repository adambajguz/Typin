namespace Typin.Internal.AutoComplete
{
    internal interface IAutoCompleteHandler
    {
        char[] Separators { get; set; }
        string[] GetSuggestions(string text, int index);
    }
}
