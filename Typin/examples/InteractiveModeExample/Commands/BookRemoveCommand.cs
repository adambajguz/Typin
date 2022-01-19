namespace InteractiveModeExample.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using InteractiveModeExample.Services;
    using Typin.Commands;
    using Typin.Console;
    using Typin.Models.Attributes;
    using Typin.Schemas.Attributes;

    [Alias("book remove")]
    public class BookRemoveCommand : ICommand
    {
        private readonly LibraryService _libraryService;
        private readonly IConsole _console;

        [Parameter(0, Name = "title", Description = "Book title.")]
        public string Title { get; init; } = string.Empty;

        public BookRemoveCommand(LibraryService libraryService, IConsole console)
        {
            _libraryService = libraryService;
            _console = console;
        }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            var book = _libraryService.GetBook(Title);

            if (book == null)
            {
                throw new Exception("Book not found.");
            }

            _libraryService.RemoveBook(book);

            _console.Output.WriteLine($"Book {Title} removed.");

            return default;
        }
    }
}