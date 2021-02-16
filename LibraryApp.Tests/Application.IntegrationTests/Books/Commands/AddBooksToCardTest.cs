using FluentAssertions;
using LibraryApp.Application.Books.Commands.AddBooksToCard;
using LibraryApp.Application.Books.Commands.AddBookToLibrary;
using LibraryApp.Application.Common.Enums;
using LibraryApp.Domain.Entities;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryApp.Tests.Application.IntegrationTests.Books.Commands
{
    using static ScopedRequest;

    class AddBooksToCardTest : BaseTest
    {
        private const int _bookId = 1;
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
            var addBookToLibraryCmd = new AddBookToLibraryCommand()
            {
                Title = _title,
                Authors = new List<string>() { _author_1, _author_2 }
            };
            var addToLibraryResult = await SendAsync(addBookToLibraryCmd);

            var addBookToCardCmd = new AddBooksToCardCommand(_bookId, username);

            var result = await SendAsync(addBookToCardCmd);

            result.Succeeded.Should().BeTrue();
        }
    }
}
