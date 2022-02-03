namespace Typin.Console
{
    public partial class BaseConsole
    {
        /// <inheritdoc />
        public virtual int BufferWidth
        {
            get
            {
                ValidateFeature(ConsoleFeatures.BufferSize);

                return int.MaxValue;
            }

            set
            {
                ValidateFeature(ConsoleFeatures.BufferSize);
            }
        }

        /// <inheritdoc />
        public virtual int BufferHeight
        {
            get
            {
                ValidateFeature(ConsoleFeatures.BufferSize);

                return int.MaxValue;
            }

            set
            {
                ValidateFeature(ConsoleFeatures.BufferSize);
            }
        }

        /// <inheritdoc />
        public virtual void SetBufferSize(int width, int height)
        {
            ValidateFeature(ConsoleFeatures.BufferSize);
        }
    }
}