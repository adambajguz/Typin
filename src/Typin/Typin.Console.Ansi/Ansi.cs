namespace Typin.Console
{
    using System;

    /// <summary>
    /// See https://misc.flogisoft.com/bash/tip_colors_and_formatting for more info about ANSI/VT100 terminals commands.
    /// </summary>
    public static class Ansi
    {
        /// <summary>
        /// Escape character.
        /// </summary>
        public static string Esc { get; } = "\u001b";

        /// <summary>
        /// Text formatting.
        /// </summary>
        public static class Text
        {
            /// <summary>
            /// Resets all attributes.
            /// </summary>
            public static string AttributesOff { get; } = $"{Esc}[0m";

            /// <summary>
            /// Blink.
            /// </summary>
            public static class Blink
            {
                /// <summary>
                /// Blink off.
                /// </summary>
                public static string Off { get; } = $"{Esc}[25m";

                /// <summary>
                /// Blink on.
                /// </summary>
                public static string On { get; } = $"{Esc}[5m";
            }

            /// <summary>
            /// Bold.
            /// </summary>
            public static class Bold
            {
                /// <summary>
                /// Bold off.
                /// </summary>
                public static string Off { get; } = $"{Esc}[22m";

                /// <summary>
                /// Bold on.
                /// </summary>
                public static string On { get; } = $"{Esc}[1m";
            }

            /// <summary>
            /// Hidden.
            /// </summary>
            public static class Hidden
            {
                /// <summary>
                /// Hidden on.
                /// </summary>
                public static string On { get; } = $"{Esc}[8m";

                /// <summary>
                /// Hidden off.
                /// </summary>
                public static string Off { get; } = $"{Esc}[28m";
            }

            /// <summary>
            /// Reverse.
            /// </summary>
            public static class Reverse
            {
                /// <summary>
                /// Reverse on.
                /// </summary>
                public static string On { get; } = $"{Esc}[7m";

                /// <summary>
                /// Reverse off.
                /// </summary>
                public static string Off { get; } = $"{Esc}[27m";
            }

            /// <summary>
            /// Underlined.
            /// </summary>
            public static class Underlined
            {
                /// <summary>
                /// Underlined off.
                /// </summary>
                public static string Off { get; } = $"{Esc}[24m";

                /// <summary>
                /// Underlined on.
                /// </summary>
                public static string On { get; } = $"{Esc}[4m";
            }
        }

        /// <summary>
        /// Colors.
        /// </summary>
        public static class Color
        {
            /// <summary>
            /// Background colors.
            /// </summary>
            public static class Background
            {
                /// <summary>
                /// Default color.
                /// </summary>
                public static string Default { get; } = $"{Esc}[49m";

                /// <summary>
                /// <see cref="ConsoleColor.Black"/>.
                /// </summary>
                public static string Black => $"{Esc}[40m";

                /// <summary>
                /// <see cref="ConsoleColor.DarkRed"/>.
                /// </summary>
                public static string Red { get; } = $"{Esc}[41m";

                /// <summary>
                /// <see cref="ConsoleColor.DarkGreen"/>.
                /// </summary>
                public static string Green { get; } = $"{Esc}[42m";

                /// <summary>
                /// <see cref="ConsoleColor.DarkYellow"/>.
                /// </summary>
                public static string Yellow { get; } = $"{Esc}[43m";

                /// <summary>
                /// <see cref="ConsoleColor.DarkBlue"/>.
                /// </summary>
                public static string Blue { get; } = $"{Esc}[44m";

                /// <summary>
                /// <see cref="ConsoleColor.DarkMagenta"/>.
                /// </summary>
                public static string Magenta { get; } = $"{Esc}[45m";

                /// <summary>
                /// <see cref="ConsoleColor.DarkCyan"/>.
                /// </summary>
                public static string Cyan { get; } = $"{Esc}[46m";

                /// <summary>
                /// <see cref="ConsoleColor.Gray"/>.
                /// </summary>
                public static string LightGray { get; } = $"{Esc}[47m";

                /// <summary>
                /// <see cref="ConsoleColor.DarkGray"/>.
                /// </summary>
                public static string DarkGray { get; } = $"{Esc}[100m";

                /// <summary>
                /// <see cref="ConsoleColor.Red"/>.
                /// </summary>
                public static string LightRed { get; } = $"{Esc}[101m";

                /// <summary>
                /// <see cref="ConsoleColor.Green"/>.
                /// </summary>
                public static string LightGreen { get; } = $"{Esc}[102m";

                /// <summary>
                /// <see cref="ConsoleColor.Yellow"/>.
                /// </summary>
                public static string LightYellow { get; } = $"{Esc}[103m";

                /// <summary>
                /// <see cref="ConsoleColor.Blue"/>.
                /// </summary>
                public static string LightBlue { get; } = $"{Esc}[104m";

                /// <summary>
                /// <see cref="ConsoleColor.Magenta"/>.
                /// </summary>
                public static string LightMagenta { get; } = $"{Esc}[105m";

                /// <summary>
                /// <see cref="ConsoleColor.Cyan"/>.
                /// </summary>
                public static string LightCyan { get; } = $"{Esc}[106m";

                /// <summary>
                /// <see cref="ConsoleColor.White"/>.
                /// </summary>
                public static string White { get; } = $"{Esc}[107m";

                /// <summary>
                /// RGB background color command.
                /// </summary>
                /// <param name="r"></param>
                /// <param name="g"></param>
                /// <param name="b"></param>
                /// <returns></returns>
                public static string Rgb(byte r, byte g, byte b)
                {
                    return $"{Esc}[48;2;{r};{g};{b}m";
                }

                /// <summary>
                /// Background color command from <see cref="ConsoleColor"/>/
                /// </summary>
                /// <param name="background"></param>
                /// <returns></returns>
                public static string FromConsoleColor(ConsoleColor background)
                {
                    return background switch
                    {
                        ConsoleColor.Black => Black,
                        ConsoleColor.DarkRed => Red,
                        ConsoleColor.DarkGreen => Green,
                        ConsoleColor.DarkYellow => Yellow,
                        ConsoleColor.DarkBlue => Blue,
                        ConsoleColor.DarkMagenta => Magenta,
                        ConsoleColor.DarkCyan => Cyan,
                        ConsoleColor.Gray => LightGray,
                        ConsoleColor.DarkGray => DarkGray,
                        ConsoleColor.Red => LightRed,
                        ConsoleColor.Green => LightGreen,
                        ConsoleColor.Yellow => LightYellow,
                        ConsoleColor.Blue => LightBlue,
                        ConsoleColor.Magenta => LightMagenta,
                        ConsoleColor.Cyan => LightCyan,
                        ConsoleColor.White => White,
                        _ => Default
                    };
                }
            }

            /// <summary>
            /// Foreground colors.
            /// </summary>
            public static class Foreground
            {
                /// <summary>
                /// Default foreground.
                /// </summary>
                public static string Default => $"{Esc}[39m";

                /// <summary>
                /// <see cref="ConsoleColor.Black"/>
                /// </summary>
                public static string Black { get; } = $"{Esc}[30m";

                /// <summary>
                /// <see cref="ConsoleColor.DarkRed"/>
                /// </summary>
                public static string Red { get; } = $"{Esc}[31m";

                /// <summary>
                /// <see cref="ConsoleColor.DarkGreen"/>
                /// </summary>
                public static string Green { get; } = $"{Esc}[32m";

                /// <summary>
                /// <see cref="ConsoleColor.DarkYellow"/>
                /// </summary>
                public static string Yellow { get; } = $"{Esc}[33m";

                /// <summary>
                /// <see cref="ConsoleColor.DarkBlue"/>
                /// </summary>
                public static string Blue { get; } = $"{Esc}[34m";

                /// <summary>
                /// <see cref="ConsoleColor.DarkMagenta"/>
                /// </summary>
                public static string Magenta { get; } = $"{Esc}[35m";

                /// <summary>
                /// <see cref="ConsoleColor.DarkCyan"/>
                /// </summary>
                public static string Cyan { get; } = $"{Esc}[36m";

                /// <summary>
                /// <see cref="ConsoleColor.Gray"/>
                /// </summary>
                public static string LightGray { get; } = $"{Esc}[37m";

                /// <summary>
                /// <see cref="ConsoleColor.DarkGray"/>
                /// </summary>
                public static string DarkGray { get; } = $"{Esc}[90m";

                /// <summary>
                /// <see cref="ConsoleColor.Red"/>
                /// </summary>
                public static string LightRed { get; } = $"{Esc}[91m";

                /// <summary>
                /// <see cref="ConsoleColor.Green"/>
                /// </summary>
                public static string LightGreen { get; } = $"{Esc}[92m";

                /// <summary>
                /// <see cref="ConsoleColor.Yellow"/>
                /// </summary>
                public static string LightYellow { get; } = $"{Esc}[93m";

                /// <summary>
                /// <see cref="ConsoleColor.Blue"/>
                /// </summary>
                public static string LightBlue { get; } = $"{Esc}[94m";

                /// <summary>
                /// <see cref="ConsoleColor.Magenta"/>
                /// </summary>
                public static string LightMagenta { get; } = $"{Esc}[95m";

                /// <summary>
                /// <see cref="ConsoleColor.Cyan"/>
                /// </summary>
                public static string LightCyan { get; } = $"{Esc}[96m";

                /// <summary>
                /// <see cref="ConsoleColor.White"/>
                /// </summary>
                public static string White { get; } = $"{Esc}[97m";

                /// <summary>
                /// RGB foreground color command.
                /// </summary>
                /// <param name="r"></param>
                /// <param name="g"></param>
                /// <param name="b"></param>
                /// <returns></returns>
                public static string Rgb(byte r, byte g, byte b)
                {
                    return $"{Esc}[38;2;{r};{g};{b}m";
                }

                /// <summary>
                /// Foreground color command from <see cref="ConsoleColor"/>/
                /// </summary>
                /// <param name="foreground"></param>
                /// <returns></returns>
                public static string FromConsoleColor(ConsoleColor foreground)
                {
                    return foreground switch
                    {
                        ConsoleColor.Black => Black,
                        ConsoleColor.DarkRed => Red,
                        ConsoleColor.DarkGreen => Green,
                        ConsoleColor.DarkYellow => Yellow,
                        ConsoleColor.DarkBlue => Blue,
                        ConsoleColor.DarkMagenta => Magenta,
                        ConsoleColor.DarkCyan => Cyan,
                        ConsoleColor.Gray => LightGray,
                        ConsoleColor.DarkGray => DarkGray,
                        ConsoleColor.Red => LightRed,
                        ConsoleColor.Green => LightGreen,
                        ConsoleColor.Yellow => LightYellow,
                        ConsoleColor.Blue => LightBlue,
                        ConsoleColor.Magenta => LightMagenta,
                        ConsoleColor.Cyan => LightCyan,
                        ConsoleColor.White => White,
                        _ => Default
                    };
                }
            }
        }

        /// <summary>
        /// Cursor commands.
        /// </summary>
        public static class Cursor
        {
            /// <summary>
            /// Cursor movement.
            /// </summary>
            public static class Move
            {
                /// <summary>
                /// Moves cursor up.
                /// </summary>
                /// <param name="lines"></param>
                /// <returns></returns>
                public static string Up(int lines = 1)
                {
                    return $"{Esc}[{lines}A";
                }

                /// <summary>
                /// Moves cursor down.
                /// </summary>
                /// <param name="lines"></param>
                /// <returns></returns>
                public static string Down(int lines = 1)
                {
                    return $"{Esc}[{lines}B";
                }

                /// <summary>
                /// Moves cursor right.
                /// </summary>
                /// <param name="columns"></param>
                /// <returns></returns>
                public static string Right(int columns = 1)
                {
                    return $"{Esc}[{columns}C";
                }

                /// <summary>
                /// Moves cursor left.
                /// </summary>
                /// <param name="columns"></param>
                /// <returns></returns>
                public static string Left(int columns = 1)
                {
                    return $"{Esc}[{columns}D";
                }

                /// <summary>
                /// Moves cursor to next line.
                /// </summary>
                /// <param name="line"></param>
                /// <returns></returns>
                public static string NextLine(int line = 1)
                {
                    return $"{Esc}[{line}E";
                }

                /// <summary>
                /// Moves cursor to left corner.
                /// </summary>
                public static string ToUpperLeftCorner { get; } = $"{Esc}[H";

                /// <summary>
                /// Moves cursor to location.
                /// </summary>
                /// <param name="left"></param>
                /// <param name="top"></param>
                /// <returns></returns>
                public static string ToLocation(int? left = null, int? top = null)
                {
                    return $"{Esc}[{top ?? 1};{left ?? 1}H";
                }
            }

            /// <summary>
            /// Cursor scrolling.
            /// </summary>
            public static class Scroll
            {
                /// <summary>
                /// Scrolls one up.
                /// </summary>
                public static string UpOne { get; } = $"{Esc}[S";

                /// <summary>
                /// Scrolls one down.
                /// </summary>

                public static string DownOne { get; } = $"{Esc}[T";
            }

            /// <summary>
            /// Hide cursor.
            /// </summary>
            public static string Hide { get; } = $"{Esc}[?25l";

            /// <summary>
            /// Show cursor.
            /// </summary>
            public static string Show { get; } = $"{Esc}[?25h";

            /// <summary>
            /// Saves cursor position.
            /// </summary>
            public static string SavePosition { get; } = $"{Esc}7";

            /// <summary>
            /// Restores cursor position.
            /// </summary>
            public static string RestorePosition { get; } = $"{Esc}8";
        }

        /// <summary>
        /// Screen clearing.
        /// </summary>
        public static class Clear
        {
            /// <summary>
            /// Clear entire screen.
            /// </summary>
            public static string EntireScreen { get; } = $"{Esc}[2J";

            /// <summary>
            /// Clear line.
            /// </summary>
            public static string Line { get; } = $"{Esc}[2K";

            /// <summary>
            /// Clear to beginning of the lind.
            /// </summary>
            public static string ToBeginningOfLine { get; } = $"{Esc}[1K";

            /// <summary>
            /// Clear to the beginning of the screen.
            /// </summary>
            public static string ToBeginningOfScreen { get; } = $"{Esc}[1J";

            /// <summary>
            /// Clear to the end of the line.
            /// </summary>
            public static string ToEndOfLine { get; } = $"{Esc}[K";

            /// <summary>
            /// Clear to the end of the screen.
            /// </summary>
            public static string ToEndOfScreen { get; } = $"{Esc}[J";
        }
    }
}
