namespace Typin.Console
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public partial class SystemConsole
    {
        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        public override int CursorLeft
        {
            get
            {
                int @default = base.CursorLeft;

                return this.IsEnabled(ConsoleFeatures.CursorPosition)
                    ? Console.CursorLeft
                    : @default;
            }

            set
            {
                base.CursorLeft = value;

                if (this.IsEnabled(ConsoleFeatures.CursorPosition))
                {
                    Console.CursorLeft = value;
                }
            }
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        public override int CursorTop
        {
            get
            {
                int @default = base.CursorTop;

                return this.IsEnabled(ConsoleFeatures.CursorPosition)
                    ? Console.CursorTop
                    : @default;
            }

            set
            {
                base.CursorTop = value;

                if (this.IsEnabled(ConsoleFeatures.CursorPosition))
                {
                    Console.CursorTop = value;
                }
            }
        }

        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        public override void SetCursorPosition(int left, int top)
        {
            base.SetCursorPosition(left, top);

            if (this.IsEnabled(ConsoleFeatures.CursorPosition))
            {
                Console.SetCursorPosition(left, top);
            }
        }
    }
}