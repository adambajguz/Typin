namespace Typin.Console
{
    using System.Diagnostics.CodeAnalysis;

    public partial class VirtualConsole
    {
        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        public override int CursorLeft
        {
            get => base.CursorLeft;
            set => base.CursorLeft = value;
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        public override int CursorTop
        {
            get => base.CursorTop;
            set => base.CursorTop = value;
        }

        /// <inheritdoc/>
        public override void SetCursorPosition(int left, int top)
        {
            base.SetCursorPosition(left, top);
        }
    }
}