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
    public class BookTests
    {
        const string title = "testTitle";
        private readonly Mock<Author> author = new Mock<Author>(string.Empty);

        [Test]
        public void SetAuthor_ShouldSetAndFlushPrevious()
        {
            var book = new Book(title);
            var oldBookAuthor = new Mock<BookAuthor>(1, author.Object, 1, book);
            var newBookAuthor = new Mock<BookAuthor>(2, author.Object, 2, book);
            book.SetAuthor(oldBookAuthor.Object);

            book.SetAuthor(newBookAuthor.Object);

            book.BookAuthors.Should().OnlyContain(ba => ba == newBookAuthor.Object);
        }

        [Test]
        public void SetSeveralAuthors_ShouldSetAndFlushPrevious()
        {
            var book = new Book(title);

            var existedBookAuthors = new List<BookAuthor>()
            {
                new Mock<BookAuthor>(1, author.Object, 1, book).Object,
                new Mock<BookAuthor>(2, author.Object, 2, book).Object
            };

            var newBookAuthors = new List<BookAuthor>()
            {
                new Mock<BookAuthor>(3, author.Object, 3, book).Object,
                new Mock<BookAuthor>(4, author.Object, 4, book).Object
            };

            book.SetAuthors(existedBookAuthors);
            book.SetAuthors(newBookAuthors);

            book.BookAuthors.Should().BeEquivalentTo(newBookAuthors);
        }

        [Test]
        public void SetCardAndTerms_ReturnDateShouldBeLaterThanReceiveDate()
        {
            var book = new Book(title);

            book.SetCardAndTerms(1);

            book.ReturnDate.Value.Should().BeAfter(book.ReceiveDate.Value);
        }
    }
}
