﻿namespace BookLibraryExample.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using BookLibraryExample.Models;
    using BookLibraryExample.Services;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Exceptions;

    [Command("book remove", Description = "Remove a book from the library.")]
    public class BookRemoveCommand : ICommand
    {
        private readonly LibraryService _libraryService;
        private readonly IConsole _console;

        [Parameter(0, Name = "title", Description = "Book title.")]
        public string Title { get; init; } = "";

        public BookRemoveCommand(LibraryService libraryService, IConsole console)
        {
            _libraryService = libraryService;
            _console = console;
        }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            Book? book = _libraryService.GetBook(Title);

            _ = book ?? throw new CommandException("Book not found.", 1);

            _libraryService.RemoveBook(book);

            _console.Output.WriteLine($"Book {Title} removed.");

            return default;
        }
    }
}