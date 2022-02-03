namespace Typin.Console
{
    using System.Diagnostics.CodeAnalysis;

    public partial class AnsiConsole
    {
        private int _cursorLeft, _cursorTop;

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        public override int CursorLeft
        {
            get
            {
                int @default = base.CursorLeft;

                return this.IsEnabled(ConsoleFeatures.CursorPosition)
                    ? _cursorLeft
                    : @default;
            }

            set
            {
                base.CursorLeft = value;

                if (this.IsEnabled(ConsoleFeatures.CursorPosition))
                {
                    _cursorLeft = value;

                    Output.Write(Ansi.Cursor.Move.ToLocation(value, null));
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
                    ? _cursorTop
                    : @default;
            }

            set
            {
                base.CursorTop = value;

                if (this.IsEnabled(ConsoleFeatures.CursorPosition))
                {
                    _cursorTop = value;

                    Output.Write(Ansi.Cursor.Move.ToLocation(null, value));
                }
            }
        }

        /// <inheritdoc/>
        public override void SetCursorPosition(int left, int top)
        {
            base.SetCursorPosition(left, top);

            if (this.IsEnabled(ConsoleFeatures.CursorPosition))
            {
                _cursorLeft = left;
                _cursorTop = top;

                Output.Write(Ansi.Cursor.Move.ToLocation(left, top));
            }
        }
    }
}