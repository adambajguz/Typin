namespace Typin.Console
{
    public partial class BaseConsole
    {
        /// <inheritdoc />
        public virtual void Clear()
        {
            ValidateFeature(ConsoleFeatures.Clear);
        }
    }
}