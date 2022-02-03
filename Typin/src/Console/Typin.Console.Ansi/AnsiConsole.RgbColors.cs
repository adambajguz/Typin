namespace Typin.Console
{
    public partial class AnsiConsole
    {
        /// <inheritdoc />
        public override void SetBackground(byte r, byte g, byte b)
        {
            base.SetBackground(r, g, b);

            if (this.IsEnabled(ConsoleFeatures.RgbColors))
            {
                Output.Write(Ansi.Color.Background.Rgb(r, g, b));
            }
        }

        /// <inheritdoc />
        public override void SetForeground(byte r, byte g, byte b)
        {
            base.SetForeground(r, g, b);

            if (this.IsEnabled(ConsoleFeatures.RgbColors))
            {
                Output.Write(Ansi.Color.Foreground.Rgb(r, g, b));
            }
        }

        /// <inheritdoc />
        public override void SetColors(byte br, byte bg, byte bb,
                                       byte fr, byte fg, byte fb)
        {
            base.SetColors(br, bg, bb, fr, fg, fb);

            if (this.IsEnabled(ConsoleFeatures.RgbColors))
            {
                Output.Write(string.Concat(Ansi.Color.Background.Rgb(br, bg, bb), Ansi.Color.Foreground.Rgb(fr, fg, fb)));
            }
        }
    }
}