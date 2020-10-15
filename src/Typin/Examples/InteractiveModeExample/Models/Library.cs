namespace InteractiveModeExample.Models
{
    using System;
    using System.Collections.Generic;

    public class Library
    {
        public static Library Empty { get; } = new Library(Array.Empty<Book>());

        public IReadOnlyList<Book> Books { get; }

        public Library(IReadOnlyList<Book> books)
        {
            Books = books;
        }
    }
}