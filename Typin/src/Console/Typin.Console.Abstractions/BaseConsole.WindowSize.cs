namespace Typin.Console
{
    public partial class BaseConsole
    {
        /// <inheritdoc/>
        public virtual int WindowWidth
        {
            get
            {
                ValidateFeature(ConsoleFeatures.WindowSize);

                return int.MaxValue;
            }

            set => ValidateFeature(ConsoleFeatures.WindowSize);
        }

        /// <inheritdoc/>
        public virtual int WindowHeight
        {
            get
            {
                ValidateFeature(ConsoleFeatures.WindowSize);

                return int.MaxValue;
            }

            set => ValidateFeature(ConsoleFeatures.WindowSize);
        }

        /// <inheritdoc />
        public virtual int LargestWindowWidth
        {
            get
            {
                ValidateFeature(ConsoleFeatures.WindowSize);

                return int.MaxValue;
            }
        }

        /// <inheritdoc />
        public virtual int LargestWindowHeight
        {
            get
            {
                ValidateFeature(ConsoleFeatures.WindowSize);

                return int.MaxValue;
            }
        }

        /// <inheritdoc />
        public virtual void SetWindowSize(int width, int height)
        {
            ValidateFeature(ConsoleFeatures.WindowSize);
        }
    }
}