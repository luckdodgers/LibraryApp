using FluentAssertions;
using LibraryApp.Application.Books.Commands.ReturnBookToLibrary;
using LibraryApp.Application.Books.Queries.GetBooksByAuthor;
using LibraryApp.Application.Common.Enums;
using LibraryApp.Application.Common.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryApp.Tests.Application.IntegrationTests.Books.Queries
{
    using static ScopedRequest;

    class GetBooksByAuthorTest : BaseTest
    {
        [Test]
        public async Task SendCommandWithDefaultFields_ShouldReturnValidationError()
        {
            var query = new GetBooksByAuthorQuery()
            {
                AuthorName = string.Empty,
                CurrentlyAvailableOnly = false
            };

            var result = await SendAsync(query);

            result.Succeeded.Should().BeFalse();
            result.ErrorType.Should().Be(RequestError.ValidationError);
        }
        
        [Test]
        public async Task SendNonExistingAuthor_ShouldReturnNotFoundError()
        {
            var query = new GetBooksByAuthorQuery()
            {
                AuthorName = "Not existing author",
                CurrentlyAvailableOnly = false
            };

            var result = await SendAsync(query);

            result.Succeeded.Should().BeFalse();
            result.ErrorType.Should().Equals(RequestError.NotFound);
        }

        [Test]
        public async Task SendValidQuery_ShouldGetAuthorsBooks()
        {
            var neededBook_1 = await Arrange.SeedBookToLibrary(Arrange.Title, Arrange.Author_1);
            var neededBook_2 = await Arrange.SeedBookToLibrary(Arrange.Title + "_2", Arrange.Author_1);
            var otherBook_3 = await Arrange.SeedBookToLibrary(Arrange.Title + "_3", Arrange.Author_2);
            var query = new GetBooksByAuthorQuery()
            {
                AuthorName = Arrange.Author_1,
                CurrentlyAvailableOnly = false
            };

            var result = await SendAsync(query);

            result.Succeeded.Should().BeTrue();
            ((QueryResult<List<LibraryBookDto>>)result).Value.Should().OnlyContain(b => b.Id == neededBook_1.Id || b.Id == neededBook_2.Id);
        }
    }
}
