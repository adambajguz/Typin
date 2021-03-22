namespace Typin.Extensions
{
    using System;

    /// <summary>
    /// ConsoleKeyInfo extensions
    /// </summary>
    public static class ConsoleKeyInfoExtensions
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static readonly ConsoleKeyInfo Backspace = new('\0', ConsoleKey.Backspace, false, false, false);
        public static readonly ConsoleKeyInfo CtrlBackspace = new('\0', ConsoleKey.Backspace, false, false, true);
        public static readonly ConsoleKeyInfo Delete = new('\0', ConsoleKey.Delete, false, false, false);
        public static readonly ConsoleKeyInfo CtrlDelete = new('\0', ConsoleKey.Delete, false, false, true);
        public static readonly ConsoleKeyInfo Enter = new('\r', ConsoleKey.Enter, false, false, false);
        public static readonly ConsoleKeyInfo Escape = new('\u001b', ConsoleKey.Escape, false, false, false);

        public static readonly ConsoleKeyInfo Home = new('\0', ConsoleKey.Home, false, false, false);
        public static readonly ConsoleKeyInfo End = new('\0', ConsoleKey.End, false, false, false);

        public static readonly ConsoleKeyInfo LeftArrow = new('\0', ConsoleKey.LeftArrow, false, false, false);
        public static readonly ConsoleKeyInfo CtrlLeftArrow = new('\0', ConsoleKey.LeftArrow, false, false, true);
        public static readonly ConsoleKeyInfo RightArrow = new('\0', ConsoleKey.RightArrow, false, false, false);
        public static readonly ConsoleKeyInfo CtrlRightArrow = new('\0', ConsoleKey.RightArrow, false, false, true);
        public static readonly ConsoleKeyInfo UpArrow = new('\0', ConsoleKey.UpArrow, false, false, false);
        public static readonly ConsoleKeyInfo DownArrow = new('\0', ConsoleKey.DownArrow, false, false, false);

        public static readonly ConsoleKeyInfo Tab = new('\0', ConsoleKey.Tab, false, false, false);
        public static readonly ConsoleKeyInfo ShiftTab = new('\0', ConsoleKey.Tab, true, false, false);

        public static readonly ConsoleKeyInfo ExclamationMark = CharExtensions.ExclamationMark.ToConsoleKeyInfo();
        public static readonly ConsoleKeyInfo Space = CharExtensions.Space.ToConsoleKeyInfo();
        public static readonly ConsoleKeyInfo CtrlA = CharExtensions.CtrlA.ToConsoleKeyInfo();
        public static readonly ConsoleKeyInfo CtrlB = CharExtensions.CtrlB.ToConsoleKeyInfo();
        public static readonly ConsoleKeyInfo CtrlD = CharExtensions.CtrlD.ToConsoleKeyInfo();
        public static readonly ConsoleKeyInfo CtrlE = CharExtensions.CtrlE.ToConsoleKeyInfo();
        public static readonly ConsoleKeyInfo CtrlF = CharExtensions.CtrlF.ToConsoleKeyInfo();
        public static readonly ConsoleKeyInfo CtrlH = CharExtensions.CtrlH.ToConsoleKeyInfo();
        public static readonly ConsoleKeyInfo CtrlK = CharExtensions.CtrlK.ToConsoleKeyInfo();
        public static readonly ConsoleKeyInfo CtrlL = CharExtensions.CtrlL.ToConsoleKeyInfo();
        public static readonly ConsoleKeyInfo CtrlN = CharExtensions.CtrlN.ToConsoleKeyInfo();
        public static readonly ConsoleKeyInfo CtrlP = CharExtensions.CtrlP.ToConsoleKeyInfo();
        public static readonly ConsoleKeyInfo CtrlT = CharExtensions.CtrlT.ToConsoleKeyInfo();
        public static readonly ConsoleKeyInfo CtrlU = CharExtensions.CtrlU.ToConsoleKeyInfo();
        public static readonly ConsoleKeyInfo CtrlW = CharExtensions.CtrlW.ToConsoleKeyInfo();
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
