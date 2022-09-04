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

    [Alias("book list")]
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
}