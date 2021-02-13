using LibraryApp.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LibraryApp.Tests.Domain.Tests.Entities
{
    public class CardTests
    {
        private const string userName = "testusername";
        private readonly Mock<Book> book = new Mock<Book>(string.Empty);

        [Fact]
        public void AddNewBookToCard_ShouldAddSuccessfully()
        {
            var card = new Card(userName);
            book.SetupGet(b => b.Id).Returns(1);

            var isSuccessful = card.TryAddBook(book.Object);

            Assert.True(isSuccessful);
            Assert.Contains(card.Books, b => b.Id == 1);
        }

        [Fact]
        public void TryToAddBookThatAlreadyInCard_ShouldReturnFail()
        {
            var card = new Card(userName);
            book.SetupGet(b => b.Id).Returns(1);
            card.TryAddBook(book.Object);

            var isSuccessful = card.TryAddBook(book.Object);

            Assert.False(isSuccessful);
            Assert.Contains(card.Books, b => b.Id == 1);
        }

        [Fact]
        public void RemoveBookThatInCard_ShouldRemoveSuccessfully()
        {
            var card = new Card(userName);
            book.SetupGet(b => b.Id).Returns(1);
            card.TryAddBook(book.Object);

            var isSuccessful = card.TryRemoveBook(book.Object);

            Assert.True(isSuccessful);
            Assert.Empty(card.Books);
        }

        [Fact]
        public void TryToRemoveBookNotInCard_ShouldReturnFail()
        {
            var card = new Card(userName);

            var isSuccessful = card.TryRemoveBook(book.Object);

            Assert.False(isSuccessful);
        }
    }
}
