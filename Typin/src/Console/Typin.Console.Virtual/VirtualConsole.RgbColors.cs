namespace Typin.Console
{
    public partial class VirtualConsole
    {
        /// <inheritdoc />
        public override void SetBackground(byte r, byte g, byte b)
        {
            base.SetBackground(r, g, b);
        }

        /// <inheritdoc />
        public override void SetForeground(byte r, byte g, byte b)
        {
            base.SetForeground(r, g, b);
        }

        /// <inheritdoc />
        public override void SetColors(byte br, byte bg, byte bb,
                                       byte fr, byte fg, byte fb)
        {
            base.SetColors(br, bg, bb, fr, fg, fb);
        }
    }
}