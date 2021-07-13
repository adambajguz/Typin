namespace InteractiveModeExample.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using InteractiveModeExample.Internal;
    using InteractiveModeExample.Services;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("book", Description = "List all books in the library.")]
    public class BookCommand : ICommand
    {
        private readonly LibraryService _libraryService;
        private readonly IConsole _console;

        public BookCommand(LibraryService libraryService, IConsole console)
        {
            _libraryService = libraryService;
            _console = console;
        }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            var library = _libraryService.GetLibrary();

            var isFirst = true;
            foreach (var book in library.Books)
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
                _console.Error.WithForegroundColor(ConsoleColor.Red, (error) => error.WriteLine("No books"));
            }

            return default;
        }
    }

    [Command("BOOK", Description = "List all books in the library.")]
    public class Book2Command : ICommand
    {
        private readonly LibraryService _libraryService;
        private readonly IConsole _console;

        public Book2Command(LibraryService libraryService, IConsole console)
        {
            _libraryService = libraryService;
            _console = console;
        }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            var library = _libraryService.GetLibrary();

            var isFirst = true;
            foreach (var book in library.Books)
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
                _console.Error.WithForegroundColor(ConsoleColor.Red, (error) => error.WriteLine("No BOOKS"));
            }

            return default;
        }
    }
}