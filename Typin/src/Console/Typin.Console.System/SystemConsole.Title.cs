namespace Typin.Console
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public partial class SystemConsole
    {
        /// <inheritdoc />
        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
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