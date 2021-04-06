namespace Typin.Tests.Extensions
{
    using System.Collections.Generic;

    internal static class TestStringExtenstions
    {
        public static string JoinToInteractiveCommand(this IEnumerable<string> commands)
        {
            return string.Join('\r', commands);
        }
    }
}
