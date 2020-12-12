namespace InteractiveModeExample.Commands
{
    using System.Threading.Tasks;
    using InteractiveModeExample.Services;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Exceptions;

    [Command("book remove", Description = "Remove a book from the library.")]
    public class BookRemoveCommand : ICommand
    {
        private readonly LibraryService _libraryService;

        [CommandParameter(0, Name = "title", Description = "Book title.")]
        public string Title { get; set; } = string.Empty;

        public BookRemoveCommand(LibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        public ValueTask ExecuteAsync(IConsole console)
        {
            var book = _libraryService.GetBook(Title);

            if (book == null)
                throw new CommandException("Book not found.", 1);

            _libraryService.RemoveBook(book);

            console.Output.WriteLine($"Book {Title} removed.");

            return default;
        }
    }
}