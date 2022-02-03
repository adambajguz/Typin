namespace Typin.Console
{
    public partial class VirtualConsole
    {
        /// <inheritdoc/>
        public override int WindowWidth
        {
            get => base.WindowWidth;
            set => base.WindowWidth = value;
        }

        /// <inheritdoc/>
        public override int WindowHeight
        {
            get => base.WindowHeight;
            set => base.WindowHeight = value;
        }

        /// <inheritdoc />
        public override int LargestWindowWidth => base.LargestWindowWidth;

        /// <inheritdoc />
        public override int LargestWindowHeight => base.LargestWindowHeight;

        /// <inheritdoc />
        public override void SetWindowSize(int width, int height)
        {
            base.SetWindowSize(width, height);
        }
    }
}