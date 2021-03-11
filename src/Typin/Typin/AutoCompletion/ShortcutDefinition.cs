namespace Typin.AutoCompletion
{
    using System;

    /// <summary>
    /// Console shortcut definition.
    /// </summary>
    public readonly struct ShortcutDefinition
    {
        /// <summary>
        /// A value that identifies the console key that was pressed.
        /// </summary>
        public ConsoleKey Key { get; }

        /// <summary>
        ///  A bitwise combination of the enumeration values that specifies one or more modifier keys pressed simultaneously with the console key.
        /// </summary>
        public ConsoleModifiers Modifiers { get; }

        /// <summary>
        /// An action associated with the shortcut.
        /// </summary>
        public Action Action { get; }

        /// <summary>
        /// Whether modifies input.
        /// </summary>
        internal bool ModifiesInput { get; }

        /// <summary>
        /// Initializes an instance of <see cref="ShortcutDefinition"/>.
        /// </summary>
        public ShortcutDefinition(ConsoleKey key, Action action)
        {
            Key = key;
            Modifiers = 0;
            Action = action;
            ModifiesInput = false;
        }

        /// <summary>
        /// Initializes an instance of <see cref="ShortcutDefinition"/>.
        /// </summary>
        internal ShortcutDefinition(ConsoleKey key, bool modifiesInput, Action action)
        {
            Key = key;
            Modifiers = 0;
            Action = action;
            ModifiesInput = modifiesInput;
        }

        /// <summary>
        /// Initializes an instance of <see cref="ShortcutDefinition"/>.
        /// </summary>
        public ShortcutDefinition(ConsoleKey key, ConsoleModifiers modifiers, Action action)
        {
            Key = key;
            Modifiers = modifiers;
            Action = action;
            ModifiesInput = false;
        }

        /// <summary>
        /// Initializes an instance of <see cref="ShortcutDefinition"/>.
        /// </summary>
        internal ShortcutDefinition(ConsoleKey key, ConsoleModifiers modifiers, bool modifiesInput, Action action)
        {
            Key = key;
            Modifiers = modifiers;
            Action = action;
            ModifiesInput = modifiesInput;
        }

        /// <inheritdoc/>
        public static bool operator ==(ShortcutDefinition left, ShortcutDefinition right)
        {
            return left.Equals(right);
        }

        /// <inheritdoc/>
        public static bool operator !=(ShortcutDefinition left, ShortcutDefinition right)
        {
            return !left.Equals(right);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is ShortcutDefinition sd && Key == sd.Key && Modifiers == sd.Modifiers;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return ((int)Key << 16) | (int)Modifiers;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (Modifiers == 0)
            {
                return Key.ToString();
            }

            return string.Concat(Modifiers, Key);
        }
    }
}
