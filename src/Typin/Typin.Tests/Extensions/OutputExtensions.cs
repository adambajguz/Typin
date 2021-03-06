﻿namespace Typin.Tests.Extensions
{
    using Typin.Utilities;
    using Xunit.Abstractions;

    internal static class OutputExtensions
    {
        public static void Print(this ITestOutputHelper output, MemoryStreamWriter stdOut, MemoryStreamWriter stdErr)
        {
            output.WriteLine("----[ STD OUT ]----");
            output.WriteLine(stdOut.GetString());

            output.WriteLine(string.Empty);
            output.WriteLine(string.Empty);

            output.WriteLine("----[ STD ERR ]----");
            output.WriteLine(stdErr.GetString());
        }
    }
}
