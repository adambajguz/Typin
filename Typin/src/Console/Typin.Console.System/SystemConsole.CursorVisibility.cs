namespace Typin.Console
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public partial class SystemConsole
    {
        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
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