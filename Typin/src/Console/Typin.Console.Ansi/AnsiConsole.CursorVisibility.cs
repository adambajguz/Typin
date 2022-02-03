namespace Typin.Console
{
    public partial class AnsiConsole
    {
        bool _cursorVisible = true;

        /// <inheritdoc />
        public override bool CursorVisible
        {
            get
            {
                bool @default = base.CursorVisible;

                return this.IsEnabled(ConsoleFeatures.CursorVisibility)
                    ? _cursorVisible
                    : @default;
            }

            set
            {
                base.CursorVisible = _cursorVisible;

                if (this.IsEnabled(ConsoleFeatures.CursorVisibility))
                {
                    _cursorVisible = value;

                    Output.Write(value ? Ansi.Cursor.Show : Ansi.Cursor.Hide);
                }
            }
        }
    }
}