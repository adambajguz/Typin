namespace Typin.Extensions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Char extensions for ConsoleKeyInfo
    /// </summary>
    public static class CharExtensions
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public const char ExclamationMark = '!';
        public const char Space = ' ';
        public const char CarriageReturn = '\r';
        public const char LineFeed = '\n';

        public const char CtrlA = '\u0001';
        public const char CtrlB = '\u0002';
        public const char CtrlD = '\u0004';
        public const char CtrlE = '\u0005';
        public const char CtrlF = '\u0006';
        public const char CtrlG = '\u0007';
        public const char CtrlH = '\u0008';
        public const char CtrlK = '\u000B';
        public const char CtrlL = '\u000C';
        public const char CtrlN = '\u000E';
        public const char CtrlP = '\u0010';
        public const char CtrlT = '\u0014';
        public const char CtrlU = '\u0015';
        public const char CtrlW = '\u0017';

        public const ConsoleModifiers NoModifiers = 0;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        private static readonly Dictionary<char, Tuple<ConsoleKey, ConsoleModifiers>> specialKeyCharMap = new()
        {
            { ExclamationMark, Tuple.Create(ConsoleKey.D0, NoModifiers) },
            { CarriageReturn, Tuple.Create(ConsoleKey.Enter, NoModifiers) },
            { LineFeed, Tuple.Create(ConsoleKey.Enter, NoModifiers) },
            { Space, Tuple.Create(ConsoleKey.Spacebar, NoModifiers) },
            { CtrlA, Tuple.Create(ConsoleKey.A, ConsoleModifiers.Control) },
            { CtrlB, Tuple.Create(ConsoleKey.B, ConsoleModifiers.Control) },
            { CtrlD, Tuple.Create(ConsoleKey.D, ConsoleModifiers.Control) },
            { CtrlE, Tuple.Create(ConsoleKey.E, ConsoleModifiers.Control) },
            { CtrlF, Tuple.Create(ConsoleKey.F, ConsoleModifiers.Control) },
            { CtrlG, Tuple.Create(ConsoleKey.G, ConsoleModifiers.Control) },
            { CtrlH, Tuple.Create(ConsoleKey.H, ConsoleModifiers.Control) },
            { CtrlK, Tuple.Create(ConsoleKey.K, ConsoleModifiers.Control) },
            { CtrlL, Tuple.Create(ConsoleKey.L, ConsoleModifiers.Control) },
            { CtrlN, Tuple.Create(ConsoleKey.N, ConsoleModifiers.Control) },
            { CtrlP, Tuple.Create(ConsoleKey.P, ConsoleModifiers.Control) },
            { CtrlT, Tuple.Create(ConsoleKey.T, ConsoleModifiers.Control) },
            { CtrlU, Tuple.Create(ConsoleKey.U, ConsoleModifiers.Control) },
            { CtrlW, Tuple.Create(ConsoleKey.W, ConsoleModifiers.Control) }
        };

        /// <summary>
        /// Converts char to ConsoleKeyInfo
        /// </summary>
        public static ConsoleKeyInfo ToConsoleKeyInfo(this char c)
        {
            (ConsoleKey key, ConsoleModifiers modifiers) = c.ParseKeyInfo();

            bool ctrl = modifiers.HasFlag(ConsoleModifiers.Control);
            bool shift = modifiers.HasFlag(ConsoleModifiers.Shift);
            bool alt = modifiers.HasFlag(ConsoleModifiers.Alt);

            return new ConsoleKeyInfo(c, key, shift, alt, ctrl);
        }

        private static Tuple<ConsoleKey, ConsoleModifiers> ParseKeyInfo(this char c)
        {
            if (Enum.TryParse(c.ToString().ToUpper(), out ConsoleKey r0))
            {
                return Tuple.Create(r0, NoModifiers);
            }

            if (specialKeyCharMap.TryGetValue(c, out Tuple<ConsoleKey, ConsoleModifiers>? r1))
            {
                return r1;
            }

            //if all else fails, return whatever the default is
            return Tuple.Create(default(ConsoleKey), NoModifiers);
        }
    }
}
