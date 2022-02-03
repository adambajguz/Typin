namespace Typin.Console
{
    using System;

    public partial class AnsiConsole
    {
        private ConsoleColor _foregroundColor = ConsoleColor.White, _backgroundColor = ConsoleColor.Black;

        /// <inheritdoc/>
        public override ConsoleColor ForegroundColor
        {
            get
            {
                ConsoleColor @default = base.ForegroundColor;

                return this.IsEnabled(ConsoleFeatures.ConsoleColors)
                    ? _foregroundColor
                    : @default;
            }

            set
            {
                base.ForegroundColor = value;

                if (this.IsEnabled(ConsoleFeatures.ConsoleColors))
                {
                    _foregroundColor = value < ConsoleColor.Black || value > ConsoleColor.White ? ConsoleColor.White : value;

                    Output.Write(Ansi.Color.Foreground.FromConsoleColor(value));
                }
            }
        }

        /// <inheritdoc/>
        public override ConsoleColor BackgroundColor
        {
            get
            {
                ConsoleColor @default = base.BackgroundColor;

                return this.IsEnabled(ConsoleFeatures.ConsoleColors)
                    ? _backgroundColor
                    : @default;
            }

            set
            {
                base.BackgroundColor = value;

                if (this.IsEnabled(ConsoleFeatures.ConsoleColors))
                {
                    _backgroundColor = value < ConsoleColor.Black || value > ConsoleColor.White ? ConsoleColor.Black : value;

                    Output.Write(Ansi.Color.Background.FromConsoleColor(value));
                }
            }
        }

        /// <inheritdoc />
        public override void SetColors(ConsoleColor background, ConsoleColor foreground)
        {
            base.SetColors(background, foreground);

            if (this.IsEnabled(ConsoleFeatures.ConsoleColors))
            {
                _foregroundColor = background;
                _backgroundColor = foreground;

                Output.Write(string.Concat(background, foreground));
            }
        }

        /// <inheritdoc/>
        public override void ResetColor()
        {
            base.ResetColor();

            if (this.IsEnabled(ConsoleFeatures.ConsoleColors) || this.IsEnabled(ConsoleFeatures.RgbColors))
            {
                _foregroundColor = ConsoleColor.White;
                _backgroundColor = ConsoleColor.Black;

                Output.Write(string.Concat(Ansi.Color.Background.Default, Ansi.Color.Foreground.Default));
            }
        }
    }
}