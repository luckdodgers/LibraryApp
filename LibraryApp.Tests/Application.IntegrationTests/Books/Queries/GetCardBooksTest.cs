using FluentAssertions;
using LibraryApp.Application.Books.Queries;
using LibraryApp.Application.Books.Queries.GetCardBooks;
using LibraryApp.Application.Common.Enums;
using LibraryApp.Application.Common.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryApp.Tests.Application.IntegrationTests.Books.Queries
{
    using static ScopedRequest;

    class GetCardBooksTest : BaseTest
    {
        [Test]
        public async Task SendQueryWithDefaultFields_ShouldReturnValidationError()
        {
            var query = new GetCardBooksQuery(-1, string.Empty);

            var result = await SendAsync(query);

            result.Succeeded.Should().BeFalse();
            result.ErrorType.Should().Be(RequestError.ValidationError);
        }

        [Test]
        public async Task SendValidQuery_ShouldReturnBooksFromCard()
        {
            // Arrange
            var username = await RunAsDefaultUserAsync();
            var book_1 = await Arrange.SeedBookToLibrary(Arrange.Title, Arrange.Author_1, Arrange.Author_2);
            var book_2 = await Arrange.SeedBookToLibrary(Arrange.Title + "_2", Arrange.Author_1, Arrange.Author_2);
            await AddBookToCardAsync(book_1.Id);
            await AddBookToCardAsync(book_2.Id);
            var query = new GetCardBooksQuery(UserCardId, username);

            var result = await SendAsync(query);

            result.Succeeded.Should().BeTrue();
            ((QueryResult<List<CardBookDto>>)result).Value.Should().OnlyContain(c => c.Id == book_1.Id || c.Id == book_2.Id);
        }
    }
}
