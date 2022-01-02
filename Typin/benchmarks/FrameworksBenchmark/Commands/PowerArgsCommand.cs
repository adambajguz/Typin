﻿namespace Typin.Benchmarks.FrameworksComparison.Commands
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

        public void Main()
        {

        }
    }
}