namespace Typin.Console
{
    using System;

    public partial class SystemConsole
    {
        /// <inheritdoc />
        public override string Title
        {
            get
            {
                string @default = base.Title;

                return this.IsEnabled(ConsoleFeatures.Title)
                    ? Console.Title
                    : @default;
            }

            set
            {
                base.Title = value;

                if (this.IsEnabled(ConsoleFeatures.Title))
                {
                    Console.Title = value;
                }
            }
        }
    }
}