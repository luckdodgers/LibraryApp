using FluentAssertions;
using LibraryApp.Application.Books.Commands.ReturnBookToLibrary;
using LibraryApp.Application.Common.Enums;
using NUnit.Framework;
using System.Threading.Tasks;

namespace LibraryApp.Tests.Application.IntegrationTests.Books.Commands
{
    using static ScopedRequest;

    class ReturnBookToLibraryTest : BaseTest
    {
        [Test]
        public async Task SendCommandWithDefaultFields_ShouldReturnValidationError()
        {
            var command = new ReturnBookToLibraryCommand(0, string.Empty);

            var result = await SendAsync(command);

            result.Succeeded.Should().BeFalse();
            result.ErrorType.Should().Be(RequestError.ValidationError);
        }

        [Test]
        public async Task SendValidCommand_ShouldRemoveBookFromCard()
        {
            // Arrange
            var username = await RunAsDefaultUserAsync();
            var book = await Arrange.SeedBookToLibrary(Arrange.Title, Arrange.Author_1, Arrange.Author_2);
            await AddBookToCardAsync(book.Id);
            var command = new ReturnBookToLibraryCommand(book.Id, username);

            // Act
            var result = await SendAsync(command);
            var bookRemovedFromCard = await GetBookAsync(book.Title);
            var card = await GetDefaultUserCardAsync();

            // Assert
            result.Succeeded.Should().BeTrue();
            bookRemovedFromCard.CardId.Should().BeNull();
            card.Books.Should().NotContain(b => b.Id == book.Id);
        }
    }
}
