using FluentAssertions;
using LibraryApp.Application.Books.Commands.AddBookToLibrary;
using LibraryApp.Application.Common.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Tests.Application.IntegrationTests.Books.Commands
{
    using static ScopedRequest;

    class AddBookToLibraryTest : BaseTest
    {
        private const string _duplicateAuthor = "Same author";
        private const string _duplicateTitle = "Same title";

        [Test]
        public async Task SendCommandWithDefaultFields_ShouldReturnValidationError()
        {
            var command = new AddBookToLibraryCommand()
            {
                Title = string.Empty,
                Authors = new List<string>()
            };

            var result = await SendAsync(command);

            result.Succeeded.Should().BeFalse();
            result.ErrorType.Should().Be(RequestError.ValidationError);
        }

        [Test]
        public async Task SendDuplicateBook_ShouldReturnAlreadyExistsError()
        {
            var command = new AddBookToLibraryCommand()
            {
                Authors = new List<string>() { _duplicateAuthor },
                Title = _duplicateTitle
            };
            await SendAsync(command);

            var result = await SendAsync(command); // Sending same data

            result.Succeeded.Should().BeFalse();
            result.ErrorType.Should().Equals(RequestError.AlreadyExists);
        }

        [Test]
        public async Task SendValidCommand_ShouldAddBookToLibrary()
        {
            var command = new AddBookToLibraryCommand()
            {
                Authors = new List<string>() { Arrange.Author_1, Arrange.Author_2 },
                Title = Arrange.Title
            };

            var result = await SendAsync(command);
            var book = await GetBookAsync(Arrange.Title);

            result.Succeeded.Should().BeTrue();
            book.Authors.Should().OnlyContain(a => a.Name == Arrange.Author_1 || a.Name == Arrange.Author_2);
        }
    }
}
