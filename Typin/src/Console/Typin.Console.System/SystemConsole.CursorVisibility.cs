namespace Typin.Console
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public partial class SystemConsole
    {
        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        public override bool CursorVisible
        {
            get
            {
                bool @default = base.CursorVisible;

                return this.IsEnabled(ConsoleFeatures.CursorVisibility)
                    ? Console.CursorVisible
                    : @default;
            }

            set
            {
                base.CursorVisible = value;

                if (this.IsEnabled(ConsoleFeatures.CursorVisibility))
                {
                    Console.CursorVisible = value;
                }
            }
        }
    }
}