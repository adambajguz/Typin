namespace BookLibraryExample.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using BookLibraryExample.Internal;
    using BookLibraryExample.Models;
    using BookLibraryExample.Services;
    using Typin.Commands;
    using Typin.Console;
    using Typin.Models.Attributes;
    using Typin.Schemas.Attributes;

    [Alias("book")]
    public class BookCommand : ICommand
    {
        private readonly LibraryService _libraryService;
        private readonly IConsole _console;

        [Parameter(0, Name = "title", Description = "Book title.")]
        public string Title { get; init; } = "";

        public BookCommand(LibraryService libraryService, IConsole console)
        {
            _libraryService = libraryService;
            _console = console;
        }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            Book? book = _libraryService.GetBook(Title);

            _ = book ?? throw new Exception("Book not found.");

            _console.RenderBook(book);

            return default;
        }
    }
}