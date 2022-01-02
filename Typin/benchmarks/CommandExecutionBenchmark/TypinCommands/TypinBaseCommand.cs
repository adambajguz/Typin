namespace CommandExecutionBenchmark.TypinCommands
{
    using Typin.Commands;
    using Typin.Models.Attributes;

    public abstract class TypinBaseCommand : ICommand
    {
        [Option("str", 's')]
        public string? StrOption { get; set; }

        [Option("int", 'i')]
        public int IntOption { get; set; }

        [Option("bool", 'b')]
        public bool BoolOption { get; set; }
    }
}