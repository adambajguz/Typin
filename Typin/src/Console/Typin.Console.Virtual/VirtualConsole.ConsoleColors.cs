namespace Typin.Console
{
    using System;

    public partial class VirtualConsole
    {
        /// <inheritdoc/>
        public override ConsoleColor ForegroundColor
        {
            get => base.ForegroundColor;
            set => base.ForegroundColor = value;
        }

        /// <inheritdoc/>
        public override ConsoleColor BackgroundColor
        {
            get => base.BackgroundColor;
            set => base.BackgroundColor = value;
        }

        /// <inheritdoc />
        public override void SetColors(ConsoleColor background, ConsoleColor foreground)
        {
            base.SetColors(background, foreground);
        }

        /// <inheritdoc/>
        public override void ResetColor()
        {
            base.ResetColor();
        }
    }
}