using LibraryApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Infrastructure.Persistance
{
    public static class DataSeed
    {
        public static async Task EnsureSeedData(IServiceProvider provider)
        {
            var dbContext = provider.GetRequiredService<AppDbContext>();
            await dbContext.Database.MigrateAsync();

            if (!await dbContext.Books.AnyAsync())
            {
                await SeedBooks(dbContext);
                await dbContext.SaveChangesAsync();
            }
        }

        private static async Task SeedBooks(AppDbContext context)
        {
            var book_1 = new Book("Introduction to Quantum Mechanics"); // Authors: David J. Griffiths, Darrell F. Schroeter
            var book_2 = new Book("Introduction to Electrodynamics"); // Author: David J. Griffiths

            var books_authors = new List<Author>() { new Author("David J. Griffiths"), new Author("Darrell F. Schroeter") };

            await context.Books.AddAsync(book_1);
            await context.Books.AddAsync(book_2);
            await context.Authors.AddRangeAsync(books_authors);

            await context.SaveChangesAsync();

            var bookAuthors = new List<BookAuthor>() 
            { 
                new BookAuthor(books_authors[0].Id, books_authors[0], book_1.Id, book_1),
                new BookAuthor(books_authors[0].Id, books_authors[0], book_2.Id, book_2),
                new BookAuthor(books_authors[1].Id, books_authors[1], book_1.Id, book_1),
            };

            context.BookAuthors.AddRange(bookAuthors);

            book_1.SetAuthors(bookAuthors);
            book_2.SetAuthors(bookAuthors[0]);

            books_authors[0].AddBook(bookAuthors[0]);
            books_authors[0].AddBook(bookAuthors[1]);
            books_authors[1].AddBook(bookAuthors[2]);

            await context.SaveChangesAsync();
        }
    }
}
