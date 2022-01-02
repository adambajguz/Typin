namespace BookLibraryExample.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using BookLibraryExample.Internal;
    using BookLibraryExample.Models;
    using BookLibraryExample.Services;
    using Typin.Commands;
    using Typin.Commands.Attributes;
    using Typin.Console;

    [Command("book list", Description = "List all books in the library.")]
    public class BookListCommand : ICommand
    {
        private readonly LibraryService _libraryService;
        private readonly IConsole _console;

        public BookListCommand(LibraryService libraryService, IConsole console)
        {
            _libraryService = libraryService;
            _console = console;
        }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            Library library = _libraryService.GetLibrary();

            bool isFirst = true;
            foreach (Book book in library.Books)
            {
                // Margin
                if (!isFirst)
                {
                    _console.Output.WriteLine();
                }

                isFirst = false;

                // Render book
                _console.RenderBook(book);
            }

            if (isFirst)
            {
                _console.Output.WithForegroundColor(ConsoleColor.Red, (output) => output.WriteLine("No books"));
            }

            return default;
        }
    }
}