﻿namespace InteractiveModeExample.Models
{
    using System.Linq;

    public static class Extensions
    {
        public static Library WithBook(this Library library, Book book)
        {
            var books = library.Books.ToList();
            books.Add(book);

            return new Library(books);
        }

        public static Library WithoutBook(this Library library, Book book)
        {
            Book[] books = library.Books.Where(b => b != book).ToArray();

            return new Library(books);
        }
    }
}