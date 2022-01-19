namespace FrameworksBenchmark.Commands
{
    using Typin.Commands;
    using Typin.Models.Attributes;
    using Typin.Schemas.Attributes;

    [Alias]
    public class TypinCommand : ICommand
    {
        [Option("str", 's')]
        public string? StrOption { get; set; }

        [Option("int", 'i')]
        public int IntOption { get; set; }

        [Option("bool", 'b')]
        public bool BoolOption { get; set; }
    }
}