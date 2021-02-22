using LibraryApp.Domain.Entities;
using Moq;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Tests.Application.IntegrationTests
{
    using static ScopedRequest;

    public static class Arrange
    {
        public const string Title = "Test title";
        public const string Author_1 = "Author A";
        public const string Author_2 = "Author B";

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
            return await GetBookAsync(_title);
        }
    }
}
