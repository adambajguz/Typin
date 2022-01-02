namespace FrameworksBenchmark.Commands
{
    using CommandLine;

    public class CommandLineParserCommand
    {
        [Option('s', "str")]
        public string? StrOption { get; set; }

        [Option('i', "int")]
        public int IntOption { get; set; }

        [Option('b', "bool")]
        public bool BoolOption { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static")]
        public void Execute()
        {

        }
    }
}