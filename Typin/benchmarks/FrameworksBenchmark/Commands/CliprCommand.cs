﻿namespace Typin.Benchmarks.FrameworksComparison.Commands
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

        public void Execute()
        {
        }
    }
}