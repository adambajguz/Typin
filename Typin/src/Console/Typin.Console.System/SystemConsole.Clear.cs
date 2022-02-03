namespace Typin.Console
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public partial class SystemConsole
    {
        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        public override void Clear()
        {
            base.Clear();

            if (this.IsEnabled(ConsoleFeatures.Clear))
            {
                Console.Clear();
            }
        }
    }
}