namespace Typin.Console
{
    public partial class BaseConsole
    {
        /// <inheritdoc />
        public virtual int CursorLeft
        {
            get
            {
                ValidateFeature(ConsoleFeatures.CursorPosition);

                return 0;
            }

            set => ValidateFeature(ConsoleFeatures.CursorPosition);
        }

        /// <inheritdoc />
        public virtual int CursorTop
        {
            get
            {
                ValidateFeature(ConsoleFeatures.CursorPosition);

                return 0;
            }

            set => ValidateFeature(ConsoleFeatures.CursorPosition);
        }

        /// <inheritdoc/>
        public virtual void SetCursorPosition(int left, int top)
        {
            ValidateFeature(ConsoleFeatures.CursorPosition);
        }
    }
}