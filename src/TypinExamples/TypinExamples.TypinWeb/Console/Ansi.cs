namespace TypinExamples.TypinWeb.Console
{
    using System;

    //https://misc.flogisoft.com/bash/tip_colors_and_formatting
    public static class Ansi
    {
        public static string Esc { get; } = "\u001b";

        public static class Text
        {
            public static string AttributesOff { get; } = $"{Esc}[0m";
            public static string BlinkOff { get; } = $"{Esc}[25m";
            public static string BlinkOn { get; } = $"{Esc}[5m";
            public static string BoldOff { get; } = $"{Esc}[22m";
            public static string BoldOn { get; } = $"{Esc}[1m";
            public static string HiddenOn { get; } = $"{Esc}[8m";
            public static string ReverseOn { get; } = $"{Esc}[7m";
            public static string ReverseOff { get; } = $"{Esc}[27m";
            public static string StandoutOff { get; } = $"{Esc}[23m";
            public static string StandoutOn { get; } = $"{Esc}[3m";
            public static string UnderlinedOff { get; } = $"{Esc}[24m";
            public static string UnderlinedOn { get; } = $"{Esc}[4m";
        }

        public static class Color
        {
            public static class Background
            {
                public static string Default { get; } = $"{Esc}[49m";

                public static string Black => $"{Esc}[40m";
                public static string DarkRed { get; } = $"{Esc}[41m";
                public static string DarkGreen { get; } = $"{Esc}[42m";
                public static string DarkYellow { get; } = $"{Esc}[43m";
                public static string DarkBlue { get; } = $"{Esc}[44m";
                public static string DarkMagenta { get; } = $"{Esc}[45m";
                public static string DarkCyan { get; } = $"{Esc}[46m";
                public static string Gray { get; } = $"{Esc}[47m";
                public static string DarkGray { get; } = $"{Esc}[100m";
                public static string Red { get; } = $"{Esc}[101m";
                public static string Green { get; } = $"{Esc}[102m";
                public static string Yellow { get; } = $"{Esc}[103m";
                public static string Blue { get; } = $"{Esc}[104m";
                public static string Magenta { get; } = $"{Esc}[105m";
                public static string Cyan { get; } = $"{Esc}[106m";
                public static string White { get; } = $"{Esc}[107m";

                public static string Rgb(byte r, byte g, byte b)
                {
                    return $"{Esc}[48;2;{r};{g};{b}m";
                }

                public static string FromConsoleColor(ConsoleColor background)
                {
                    return background switch
                    {
                        ConsoleColor.Black => Black,
                        ConsoleColor.DarkRed => DarkRed,
                        ConsoleColor.DarkGreen => DarkGreen,
                        ConsoleColor.DarkYellow => DarkYellow,
                        ConsoleColor.DarkBlue => DarkBlue,
                        ConsoleColor.DarkMagenta => DarkMagenta,
                        ConsoleColor.DarkCyan => DarkCyan,
                        ConsoleColor.Gray => Gray,
                        ConsoleColor.DarkGray => DarkGray,
                        ConsoleColor.Red => Red,
                        ConsoleColor.Green => Green,
                        ConsoleColor.Yellow => Yellow,
                        ConsoleColor.Blue => Blue,
                        ConsoleColor.Magenta => Magenta,
                        ConsoleColor.Cyan => Cyan,
                        ConsoleColor.White => White,
                        _ => Default
                    };
                }
            }

            public static class Foreground
            {
                public static string Default => $"{Esc}[39m";

                public static string Black { get; } = $"{Esc}[30m";
                public static string DarkRed { get; } = $"{Esc}[31m";
                public static string DarkGreen { get; } = $"{Esc}[32m";
                public static string DarkYellow { get; } = $"{Esc}[33m";
                public static string DarkBlue { get; } = $"{Esc}[34m";
                public static string DarkMagenta { get; } = $"{Esc}[35m";
                public static string DarkCyan { get; } = $"{Esc}[36m";
                public static string Gray { get; } = $"{Esc}[37m";
                public static string DarkGray { get; } = $"{Esc}[90m";
                public static string Red { get; } = $"{Esc}[91m";
                public static string Green { get; } = $"{Esc}[92m";
                public static string Yellow { get; } = $"{Esc}[93m";
                public static string Blue { get; } = $"{Esc}[94m";
                public static string Magenta { get; } = $"{Esc}[95m";
                public static string Cyan { get; } = $"{Esc}[96m";
                public static string White { get; } = $"{Esc}[97m";

                public static string Rgb(byte r, byte g, byte b)
                {
                    return $"{Esc}[38;2;{r};{g};{b}m";
                }

                public static string FromConsoleColor(ConsoleColor foreground)
                {
                    return foreground switch
                    {
                        ConsoleColor.Black => Black,
                        ConsoleColor.DarkRed => DarkRed,
                        ConsoleColor.DarkGreen => DarkGreen,
                        ConsoleColor.DarkYellow => DarkYellow,
                        ConsoleColor.DarkBlue => DarkBlue,
                        ConsoleColor.DarkMagenta => DarkMagenta,
                        ConsoleColor.DarkCyan => DarkCyan,
                        ConsoleColor.Gray => Gray,
                        ConsoleColor.DarkGray => DarkGray,
                        ConsoleColor.Red => Red,
                        ConsoleColor.Green => Green,
                        ConsoleColor.Yellow => Yellow,
                        ConsoleColor.Blue => Blue,
                        ConsoleColor.Magenta => Magenta,
                        ConsoleColor.Cyan => Cyan,
                        ConsoleColor.White => White,
                        _ => Default
                    };
                }
            }
        }

        public static class Cursor
        {
            public static class Move
            {
                public static string Up(int lines = 1)
                {
                    return $"{Esc}[{lines}A";
                }

                public static string Down(int lines = 1)
                {
                    return $"{Esc}[{lines}B";
                }

                public static string Right(int columns = 1)
                {
                    return $"{Esc}[{columns}C";
                }

                public static string Left(int columns = 1)
                {
                    return $"{Esc}[{columns}D";
                }

                public static string NextLine(int line = 1)
                {
                    return $"{Esc}[{line}E";
                }

                public static string ToUpperLeftCorner { get; } = $"{Esc}[H";
                public static string ToLocation(int? left = null, int? top = null)
                {
                    return $"{Esc}[{top ?? 1};{left ?? 1}H";
                }
            }

            public static class Scroll
            {
                public static string UpOne { get; } = $"{Esc}[S";

                public static string DownOne { get; } = $"{Esc}[T";
            }

            public static string Hide { get; } = $"{Esc}[?25l";

            public static string Show { get; } = $"{Esc}[?25h";

            public static string SavePosition { get; } = $"{Esc}7";

            public static string RestorePosition { get; } = $"{Esc}8";
        }

        public static class Clear
        {
            public static string EntireScreen { get; } = $"{Esc}[2J";
            public static string Line { get; } = $"{Esc}[2K";
            public static string ToBeginningOfLine { get; } = $"{Esc}[1K";
            public static string ToBeginningOfScreen { get; } = $"{Esc}[1J";
            public static string ToEndOfLine { get; } = $"{Esc}[K";
            public static string ToEndOfScreen { get; } = $"{Esc}[J";
        }
    }
}
