﻿using LibraryApp.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Tests.Application.IntegrationTests
{
    using static ScopedRequest;

    public static class Arrange
    {
        /// <summary>
        /// Seed book to library
        /// </summary>
        /// <returns>Seeded book from database</returns>
        public static async Task<Book> SeedBookToLibrary(string _title, params string[] authorsNames)
        {
            var authors = authorsNames.Select(a => new Mock<Author>(a));
            var book = new Mock<Book>(_title);
            book.Object.SetAuthors(authors.Select(a => a.Object));

            // Add book to database
            await AddAsync(book.Object); 

            // Return created book from DB
            return await GetBookByTitleAsync(_title);
        }
    }
}
