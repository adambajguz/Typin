namespace Typin.Console
{
    using System;

    public partial class BaseConsole
    {
        /// <inheritdoc/>
        public virtual ConsoleColor ForegroundColor
        {
            get
            {
                ValidateFeature(ConsoleFeatures.ConsoleColors);

                return ConsoleColor.White;
            }

            set => ValidateFeature(ConsoleFeatures.ConsoleColors);
        }

        /// <inheritdoc/>
        public virtual ConsoleColor BackgroundColor
        {
            get
            {
                ValidateFeature(ConsoleFeatures.ConsoleColors);

                return ConsoleColor.Black;
            }

            set => ValidateFeature(ConsoleFeatures.ConsoleColors);
        }

        /// <inheritdoc />
        public virtual void SetColors(ConsoleColor background, ConsoleColor foreground)
        {
            ValidateFeature(ConsoleFeatures.ConsoleColors);
        }

        /// <inheritdoc/>
        public virtual void ResetColor()
        {
            ValidateFeature(ConsoleFeatures.ConsoleColors, ConsoleFeatures.RgbColors);
        }
    }
}