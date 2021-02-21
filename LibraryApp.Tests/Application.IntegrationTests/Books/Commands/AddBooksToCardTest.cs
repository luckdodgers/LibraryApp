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
            // Arrange
            var username = await RunAsDefaultUserAsync();
            var bookBeforeAddToCard = await Arrange.SeedBookToLibrary(_title, _author_1, _author_2);
            var addBookToCardCmd = new AddBooksToCardCommand(bookBeforeAddToCard.Id, username);

            // Act
            var result = await SendAsync(addBookToCardCmd);          
            var bookAfterAddToCard = await GetBookAsync(_title); // Get DB book reference again, after setting it's CardId by test command
            var card = await GetDefaultUserCardAsync();

            // Assert
            result.Succeeded.Should().BeTrue();
            bookAfterAddToCard.CardId.Should().Equals(UserCardId);
            card.Books.Should().Contain(b => b.Id == bookAfterAddToCard.Id);
        }
    }
}
