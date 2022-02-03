namespace Typin.Console
{
    public partial class BaseConsole
    {
        /// <inheritdoc />
        public virtual string Title
        {
            get
            {
                ValidateFeature(ConsoleFeatures.Title);

                return string.Empty;
            }

            set
            {
                ValidateFeature(ConsoleFeatures.Title);
            }
        }
    }
}