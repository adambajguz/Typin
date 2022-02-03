namespace Typin.Console
{
    using System;

    public partial class SystemConsole
    {
        /// <inheritdoc />
        public override ConsoleColor ForegroundColor
        {
            get
            {
                ConsoleColor @default = base.ForegroundColor;

                return this.IsEnabled(ConsoleFeatures.ConsoleColors)
                    ? @default
                    : Console.ForegroundColor;
            }

            set
            {
                base.ForegroundColor = value;

                if (this.IsEnabled(ConsoleFeatures.ConsoleColors))
                {
                    Console.ForegroundColor = value;
                }
            }
        }

        /// <inheritdoc />
        public override ConsoleColor BackgroundColor
        {
            get
            {
                ConsoleColor @default = base.BackgroundColor;

                return this.IsEnabled(ConsoleFeatures.ConsoleColors)
                    ? @default
                    : Console.BackgroundColor;
            }

            set
            {
                base.BackgroundColor = value;

                if (this.IsEnabled(ConsoleFeatures.ConsoleColors))
                {
                    Console.BackgroundColor = value;
                }
            }
        }

        /// <inheritdoc />
        public override void SetColors(ConsoleColor background, ConsoleColor foreground)
        {
            base.SetColors(background, foreground);

            if (this.IsEnabled(ConsoleFeatures.ConsoleColors))
            {
                Console.BackgroundColor = background;
                Console.ForegroundColor = foreground;
            }
        }

        /// <inheritdoc />
        public override void ResetColor()
        {
            base.ResetColor();

            if (this.IsEnabled(ConsoleFeatures.ConsoleColors) || this.IsEnabled(ConsoleFeatures.RgbColors))
            {
                Console.ResetColor();
            }
        }
    }
}