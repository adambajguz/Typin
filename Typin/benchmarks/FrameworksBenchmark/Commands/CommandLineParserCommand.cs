﻿namespace Typin.Benchmarks.FrameworksComparison.Commands
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

        public void Execute()
        {

        }
    }
}