namespace Typin.Console
{
    public partial class BaseConsole
    {
        /// <inheritdoc />
        public virtual bool CursorVisible
        {
            get
            {
                ValidateFeature(ConsoleFeatures.CursorVisibility);

                return true;
            }

            set
            {
                ValidateFeature(ConsoleFeatures.CursorVisibility);
            }
        }
    }
}