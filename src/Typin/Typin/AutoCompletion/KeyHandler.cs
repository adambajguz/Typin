namespace Typin.AutoCompletion
{
    using System;
    using System.Collections.Generic;
    using Typin.Console;

    internal sealed class KeyHandler
    {
        private readonly IConsole _console;
        private readonly HashSet<ShortcutDefinition> _shortcuts;

        public delegate void UnhandledControlSequenceDetectedEventHandler(ref ConsoleKeyInfo keyInfo);
        public event UnhandledControlSequenceDetectedEventHandler? UnhandledControlSequenceDetected;

        public delegate void UnhandledKeyDetectedEventHandler(ref ConsoleKeyInfo keyInfo);
        public event UnhandledKeyDetectedEventHandler? UnhandledKeyDetected;

        public delegate void NewLineDetectedEventHandler();
        public event NewLineDetectedEventHandler? NewLineDetected;

        /// <summary>
        /// Initializes an instance of <see cref="KeyHandler"/>.
        /// </summary>
        public KeyHandler(IConsole console,
                          HashSet<ShortcutDefinition> shortcuts)
        {
            _console = console;
            _shortcuts = shortcuts;
        }

        /// <summary>
        /// Handles key input.
        /// </summary>
        public void ReadKey()
        {
            ConsoleKeyInfo keyInfo = _console.ReadKey(true);

            ReadKey(keyInfo);
        }

        /// <summary>
        /// Handles key input.
        /// </summary>
        public void ReadKey(ConsoleKeyInfo keyInfo)
        {
            ConsoleModifiers modifiers = keyInfo.Modifiers;
            if (keyInfo.Key == ConsoleKey.Enter && modifiers == 0)
            {
                NewLineDetected?.Invoke();
                return;
            }

            var input = new ShortcutDefinition(keyInfo.Key, keyInfo.Modifiers, () => { });

            if (_shortcuts.TryGetValue(input, out ShortcutDefinition shortcutDefinition))
            {
                shortcutDefinition.Action.Invoke();
            }
            else if (modifiers.HasFlag(ConsoleModifiers.Control) && !modifiers.HasFlag(ConsoleModifiers.Alt))
            {
                UnhandledControlSequenceDetected?.Invoke(ref keyInfo);
            }
            else
            {
                UnhandledKeyDetected?.Invoke(ref keyInfo);
            }
        }
    }
}
