namespace Typin.Internal.AutoComplete
{
    using System;
    using System.Collections.Generic;
    using Typin.Console;

    internal class AutoCompleteInput
    {
        private readonly IConsole _console;
        private readonly LinkedList<string> _history = new LinkedList<string>();
        private readonly KeyHandler _keyHandler;

        public bool IsHistoryEnabled { get; set; }
        public IAutoCompleteHandler? AutoCompletionHandler { private get; set; }

        public AutoCompleteInput(IConsole console)
        {
            _console = console;
            _keyHandler = new KeyHandler(console, _history, AutoCompletionHandler);
        }

        public void AddHistory(params string[] text)
        {
            foreach (string t in text)
                _history.AddLast(t);
        }

        public IReadOnlyCollection<string> GetHistory()
        {
            return _history;
        }

        public void ClearHistory()
        {
            _history.Clear();
        }

        public string Read()
        {
            string text = GetText();

            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            if (IsHistoryEnabled)
                _history.AddLast(text);

            return text;
        }

        private string GetText()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            while (keyInfo.Key != ConsoleKey.Enter)
            {
                _keyHandler.Handle(keyInfo);
                keyInfo = Console.ReadKey(true);
            }

            _console.Output.WriteLine();

            return _keyHandler.Text;
        }
    }
}
