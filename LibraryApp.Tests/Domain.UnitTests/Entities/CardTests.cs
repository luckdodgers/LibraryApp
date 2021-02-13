using LibraryApp.Domain.Entities;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace LibraryApp.Tests.Domain.Tests.Entities
{
    public class CardTests
    {
        private const string userName = "testusername";
        private readonly Mock<Book> book = new Mock<Book>(string.Empty);

        [Test]
        public void AddNewBookToCard_ShouldAddSuccessfully()
        {
            var card = new Card(userName);
            book.SetupGet(b => b.Id).Returns(1);

            var isSuccessful = card.TryAddBook(book.Object);

            isSuccessful.Should().BeTrue();
            card.Books.Should().OnlyContain(b => b.Id == 1);
        }

        [Test]
        public void TryToAddBookThatAlreadyInCard_ShouldReturnFail()
        {
            var card = new Card(userName);
            book.SetupGet(b => b.Id).Returns(1);
            card.TryAddBook(book.Object);

            var isSuccessful = card.TryAddBook(book.Object);

            isSuccessful.Should().BeFalse();
            card.Books.Should().OnlyContain(b => b.Id == 1);
        }

        [Test]
        public void RemoveBookThatInCard_ShouldRemoveSuccessfully()
        {
            var card = new Card(userName);
            book.SetupGet(b => b.Id).Returns(1);
            card.TryAddBook(book.Object);

            var isSuccessful = card.TryRemoveBook(book.Object);

            isSuccessful.Should().BeTrue();
            card.Books.Should().BeEmpty();
        }

        [Test]
        public void TryToRemoveBookNotInCard_ShouldReturnFail()
        {
            var card = new Card(userName);

            var isSuccessful = card.TryRemoveBook(book.Object);

            isSuccessful.Should().BeFalse();
        }
    }
}
