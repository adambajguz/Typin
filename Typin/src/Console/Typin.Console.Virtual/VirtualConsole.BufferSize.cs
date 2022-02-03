namespace Typin.Console
{
    public partial class VirtualConsole
    {
        /// <inheritdoc />
        public override int BufferWidth
        {
            get => base.BufferWidth;
            set => base.BufferWidth = value;
        }

        /// <inheritdoc />
        public override int BufferHeight
        {
            get => base.BufferHeight;
            set => base.BufferHeight = value;
        }

        /// <inheritdoc />
        public override void SetBufferSize(int width, int height)
        {
            base.SetBufferSize(width, height);
        }
    }
}