namespace Typin.Console
{
    using System.Diagnostics.CodeAnalysis;

    public partial class AnsiConsole
    {
        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        public override void Clear()
        {
            base.Clear();

            if (this.IsEnabled(ConsoleFeatures.Clear))
            {
                Output.Write(Ansi.Clear.EntireScreen);
            }
        }
    }
}