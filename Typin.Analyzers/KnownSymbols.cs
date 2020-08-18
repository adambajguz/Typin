namespace Typin.Analyzers
{
    using Microsoft.CodeAnalysis;
    using Typin.Analyzers.Internal;

    public static class KnownSymbols
    {
        public static bool IsSystemString(ISymbol symbol)
        {
            return symbol.DisplayNameMatches("string") || symbol.DisplayNameMatches("System.String");
        }

        public static bool IsSystemChar(ISymbol symbol)
        {
            return symbol.DisplayNameMatches("char") || symbol.DisplayNameMatches("System.Char");
        }

        public static bool IsSystemCollectionsGenericIEnumerable(ISymbol symbol)
        {
            return symbol.DisplayNameMatches("System.Collections.Generic.IEnumerable<T>");
        }

        public static bool IsSystemConsole(ISymbol symbol)
        {
            return symbol.DisplayNameMatches("System.Console");
        }

        public static bool IsConsoleInterface(ISymbol symbol)
        {
            return symbol.DisplayNameMatches("Typin.IConsole");
        }

        public static bool IsCommandInterface(ISymbol symbol)
        {
            return symbol.DisplayNameMatches("Typin.ICommand");
        }

        public static bool IsCommandAttribute(ISymbol symbol)
        {
            return symbol.DisplayNameMatches("Typin.Attributes.CommandAttribute");
        }

        public static bool IsCommandParameterAttribute(ISymbol symbol)
        {
            return symbol.DisplayNameMatches("Typin.Attributes.CommandParameterAttribute");
        }

        public static bool IsCommandOptionAttribute(ISymbol symbol)
        {
            return symbol.DisplayNameMatches("Typin.Attributes.CommandOptionAttribute");
        }
    }
}