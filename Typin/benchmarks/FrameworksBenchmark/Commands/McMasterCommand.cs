namespace FrameworksBenchmark.Commands
{
    using McMaster.Extensions.CommandLineUtils;

    public class McMasterCommand
    {
        [Option("--str|-s")]
        public string? StrOption { get; set; }

        [Option("--int|-i")]
        public int IntOption { get; set; }

        [Option("--bool|-b")]
        public bool BoolOption { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static")]
        public int OnExecute()
        {
            return 0;
        }
    }
}