namespace InteractiveModeExample.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using InteractiveModeExample.Internal;
    using InteractiveModeExample.Services;
    using Typin.Commands;
    using Typin.Commands.Attributes;
    using Typin.Console;

    [Command("book list-interactive", Description = "List books.",
             Manual = "Curabitur eros nisl, mollis et iaculis eget, dictum in massa. Aenean eu velit sit amet leo ultricies lobortis vel vel velit. Proin ultricies augue fringilla, ullamcorper nisi sed, tincidunt erat. Aenean facilisis elit in diam mollis pellentesque. Fusce tristique porta est non pulvinar. In aliquam lectus tellus, sit amet tempus lacus mollis a. Duis mattis, lorem et placerat molestie, ex enim lobortis odio, ultrices rutrum nulla dui id dui. Integer imperdiet justo ullamcorper odio rhoncus, blandit accumsan est varius. In suscipit magna metus, eget hendrerit metus sodales in.\n\n\tInteger volutpat mauris velit, et consequat mauris hendrerit at. Sed porta ullamcorper sodales. Phasellus quis tortor magna. Quisque ultricies hendrerit purus eget ullamcorper. Nunc condimentum dolor et lectus faucibus, pulvinar vulputate eros dapibus.\n\n\tNullam iaculis pulvinar arcu vulputate pretium. Nunc finibus risus vel leo facilisis pretium. Duis pulvinar sapien lacinia, vulputate dolor nec, ultricies lectus. Mauris ex dolor, feugiat eu enim eu, pretium dignissim lacus. Suspendisse fermentum sapien vitae luctus viverra. Donec et bibendum tortor. Nam eget mollis nisi. Sed ornare ornare semper. Aliquam tempus ante eu sodales viverra. Praesent elementum ipsum nec erat commodo, eu convallis lorem malesuada. Sed ligula lectus, pretium in massa nec, viverra semper nunc.\n\n\tIn dapibus metus at mi sodales, vel faucibus quam semper. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Curabitur finibus vel mi vitae placerat.")]
    public class BookListInteractiveCommand : ICommand
    {
        private readonly LibraryService _libraryService;
        private readonly IConsole _console;

        public BookListInteractiveCommand(LibraryService libraryService, IConsole console)
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
}