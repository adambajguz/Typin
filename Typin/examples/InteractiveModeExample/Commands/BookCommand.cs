namespace InteractiveModeExample.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using InteractiveModeExample.Internal;
    using InteractiveModeExample.Services;
    using Typin.Commands;
    using Typin.Console;
    using Typin.Schemas.Attributes;

    [Alias("book")]
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
            Models.Library library = _libraryService.GetLibrary();

            bool isFirst = true;
            foreach (Models.Book book in library.Books)
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

    [Alias("BOOK")]
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
            Models.Library library = _libraryService.GetLibrary();

            bool isFirst = true;
            foreach (Models.Book book in library.Books)
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