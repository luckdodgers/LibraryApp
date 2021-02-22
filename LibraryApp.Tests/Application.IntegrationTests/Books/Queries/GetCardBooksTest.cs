using FluentAssertions;
using LibraryApp.Application.Books.Commands.ReturnBookToLibrary;
using LibraryApp.Application.Books.Queries;
using LibraryApp.Application.Common.Enums;
using NUnit.Framework;
using System.Threading.Tasks;

namespace LibraryApp.Tests.Application.IntegrationTests.Books.Queries
{
    using static ScopedRequest;

    class GetCardBooksTest : BaseTest
    {
        [Test]
        public async Task SendCommandWithDefaultFields_ShouldReturnValidationError()
        {
            var query = new GetCardBooksQuery(-1, string.Empty);

            var result = await SendAsync(query);

            result.Succeeded.Should().BeFalse();
            result.ErrorType.Should().Be(RequestError.ValidationError);
        }


    }
}
