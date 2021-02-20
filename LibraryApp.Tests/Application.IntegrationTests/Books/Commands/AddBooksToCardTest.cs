using FluentAssertions;
using LibraryApp.Application.Books.Commands.AddBooksToCard;
using LibraryApp.Application.Common.Enums;
using LibraryApp.Domain.Entities;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Tests.Application.IntegrationTests.Books.Commands
{
    using static ScopedRequest;

    class AddBooksToCardTest : BaseTest
    {
        private const string _title = "Test title";
        private const string _author_1 = "Author A";
        private const string _author_2 = "Author B";

        [Test]
        public async Task SendCommandWithDefaultFields_ShouldReturnValidationError()
        {
            var command = new AddBooksToCardCommand(default, string.Empty);

            var result = await SendAsync(command);

            result.Succeeded.Should().BeFalse();
            result.ErrorType.Should().Be(RequestError.ValidationError);
        }

        [Test]
        public async Task SendValidCommand_ShouldAddBookToCard()
        {
            var username = await RunAsDefaultUserAsync();
            var authors = new List<Mock<Author>>()
            {
                new Mock<Author>(_author_1),
                new Mock<Author>(_author_2)
            };
            var book = new Mock<Book>(_title);
            book.Object.SetAuthors(authors.Select(a => a.Object));
            await AddAsync(book.Object);
            var bookFromDb = await GetBookByTitleAsync(_title);
            var addBookToCardCmd = new AddBooksToCardCommand(bookFromDb.Id, username);

            var result = await SendAsync(addBookToCardCmd);

            result.Succeeded.Should().BeTrue();
        }
    }
}
