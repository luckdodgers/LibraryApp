using FluentAssertions;
using LibraryApp.Application.Books.Commands.AddBookToLibrary;
using LibraryApp.Application.Common.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryApp.Application.Common.Models;
using MediatR;
using LibraryApp.Application.Books.Commands.ReturnBookToLibrary;
using LibraryApp.Application.Books.Commands.AddBooksToCard;

namespace LibraryApp.Tests.Application.IntegrationTests.Books.Commands
{
    using static ScopedRequest;

    class ReturnBookToLibraryTest : BaseTest
    {
        private const string _title = "Test title";
        private const string _author_1 = "Author A";
        private const string _author_2 = "Author B";

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
            var book = await Arrange.SeedBookToLibrary(_title, _author_1, _author_2);
            await AddBookToCardAsync(book);
            var command = new AddBooksToCardCommand(book.Id, username);

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
