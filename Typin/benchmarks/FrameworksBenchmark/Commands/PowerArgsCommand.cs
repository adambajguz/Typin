namespace FrameworksBenchmark.Commands
{
    using PowerArgs;

    public class PowerArgsCommand
    {
        [ArgShortcut("--str"), ArgShortcut("-s")]
        public string? StrOption { get; set; }

        [ArgShortcut("--int"), ArgShortcut("-i")]
        public int IntOption { get; set; }

        [ArgShortcut("--bool"), ArgShortcut("-b")]
        public bool BoolOption { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static")]
        public void Main()
        {

        }
    }
}