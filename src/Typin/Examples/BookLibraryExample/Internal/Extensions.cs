namespace BookLibraryExample.Internal
{
    using System;
    using BookLibraryExample.Models;
    using Typin.Console;

    internal static class Extensions
    {
        public static void RenderBook(this IConsole console, Book book)
        {
            // Title
            console.Output.WithForegroundColor(ConsoleColor.White, (output) => output.WriteLine(book.Title));

            // Author
            console.Output.Write("  ");
            console.Output.Write("Author: ");
            console.Output.WithForegroundColor(ConsoleColor.White, (output) => output.WriteLine(book.Author));

            // Published
            console.Output.Write("  ");
            console.Output.Write("Published: ");
            console.Output.WithForegroundColor(ConsoleColor.White, (output) => output.WriteLine($"{book.Published:d}"));

            // ISBN
            console.Output.Write("  ");
            console.Output.Write("ISBN: ");
            console.Output.WithForegroundColor(ConsoleColor.White, (output) => output.WriteLine(book.Isbn));
        }
    }
}