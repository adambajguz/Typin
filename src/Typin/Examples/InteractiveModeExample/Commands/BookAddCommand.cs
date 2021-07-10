﻿namespace InteractiveModeExample.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using InteractiveModeExample.Internal;
    using InteractiveModeExample.Models;
    using InteractiveModeExample.Services;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Exceptions;

    [Command("book add", Description = "Add a book to the library.",
            Manual = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer euismod nunc lorem, vitae cursus sem facilisis ut. Cras et nibh justo. Mauris eu elit lectus. Suspendisse potenti. Mauris luctus sapien quis arcu semper, vel venenatis elit ultrices. Quisque suscipit arcu vel massa vestibulum dapibus. Maecenas felis lacus, pharetra sed fermentum in, molestie vel ipsum. Nullam elementum arcu eget est tempor, blandit lacinia odio facilisis. Proin nulla odio, sodales et tellus nec, pulvinar ultrices nunc. Integer ornare, odio vel tincidunt congue, diam lorem facilisis lectus, id tempor sapien nibh vitae justo. Mauris ut odio justo. Etiam sed felis tellus. Nam sollicitudin neque in tempor scelerisque. Praesent sit amet nisi quis justo scelerisque placerat.")]
    public class BookAddCommand : ICommand
    {
        private readonly LibraryService _libraryService;
        private readonly IConsole _console;

        [CommandParameter(0, Name = "title", Description = "Book title.")]
        public string Title { get; init; } = "";

        [CommandOption("author", 'a', IsRequired = true, Description = "Book author.")]
        public string Author { get; init; } = "";

        [CommandOption("published", 'p', Description = "Book publish date.")]
        public DateTimeOffset Published { get; init; } = CreateRandomDate();

        [CommandOption("isbn", 'n', Description = "Book ISBN.")]
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
                throw new CommandException("Book already exists.", 1);
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