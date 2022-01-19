namespace InteractiveModeExample.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using InteractiveModeExample.Internal;
    using InteractiveModeExample.Models;
    using InteractiveModeExample.Services;
    using Typin.Commands;
    using Typin.Console;
    using Typin.Models.Attributes;
    using Typin.Schemas.Attributes;

    [Alias("book add")]
    public class BookAddCommand : ICommand
    {
        private readonly LibraryService _libraryService;
        private readonly IConsole _console;

        [Parameter(0, Name = "title", Description = "Book title.")]
        public string Title { get; init; } = "";

        [Option("author", 'a', IsRequired = true, Description = "Book author.")]
        public string Author { get; init; } = "";

        [Option("published", 'p', Description = "Book publish date.")]
        public DateTimeOffset Published { get; init; } = CreateRandomDate();

        [Option("isbn", 'n', Description = "Book ISBN.")]
        public Isbn Isbn { get; init; } = CreateRandomIsbn();

        public BookAddCommand(LibraryService libraryService, IConsole console)
        {
            _libraryService = libraryService;
            _console = console;
        }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            if (_libraryService.GetBook(Title) is not null)
            {
                throw new Exception("Book already exists.");
            }

            Book book = new(Title, Author, Published, Isbn);
            _libraryService.AddBook(book);

            _console.Output.WriteLine("Book added.");
            _console.RenderBook(book);

            return default;
        }

        #region Helpers
        private static readonly Random Random = new();

        private static DateTimeOffset CreateRandomDate()
        {
            return new DateTimeOffset(
                       Random.Next(1800, 2020),
                       Random.Next(1, 12),
                       Random.Next(1, 28),
                       Random.Next(1, 23),
                       Random.Next(1, 59),
                       Random.Next(1, 59),
                       TimeSpan.Zero);
        }

        private static Isbn CreateRandomIsbn()
        {
            return new Isbn(
                       Random.Next(0, 999),
                       Random.Next(0, 99),
                       Random.Next(0, 99999),
                       Random.Next(0, 99),
                       Random.Next(0, 9));
        }
        #endregion
    }
}