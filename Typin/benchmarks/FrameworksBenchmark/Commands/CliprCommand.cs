namespace FrameworksBenchmark.Commands
{
    using clipr;

    public class CliprCommand
    {
        [NamedArgument('s', "str")]
        public string? StrOption { get; set; }

        [NamedArgument('i', "int")]
        public int IntOption { get; set; }

        [NamedArgument('b', "bool", Constraint = NumArgsConstraint.Optional, Const = true)]
        public bool BoolOption { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static")]
        public void Execute()
        {

        }
    }
}