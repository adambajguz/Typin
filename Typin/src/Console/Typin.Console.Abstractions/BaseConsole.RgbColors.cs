namespace Typin.Console
{
    public partial class BaseConsole
    {
        /// <inheritdoc />
        public virtual void SetBackground(byte r, byte g, byte b)
        {
            ValidateFeature(ConsoleFeatures.RgbColors);
        }

        /// <inheritdoc />
        public virtual void SetForeground(byte r, byte g, byte b)
        {
            ValidateFeature(ConsoleFeatures.RgbColors);
        }

        /// <inheritdoc />
        public virtual void SetColors(byte br, byte bg, byte bb,
                                      byte fr, byte fg, byte fb)
        {
            ValidateFeature(ConsoleFeatures.RgbColors);
        }
    }
}