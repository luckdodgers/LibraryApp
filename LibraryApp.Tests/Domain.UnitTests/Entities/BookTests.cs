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
    public class BookTests
    {
        const string title = "testTitle";
        private readonly Mock<Author> author = new Mock<Author>(string.Empty);

        [Fact]
        public void SetAuthor_ShouldSetAndFlushPrevious()
        {
            var book = new Book(title);
            var oldBookAuthor = new Mock<BookAuthor>(1, author.Object, 1, book);
            var newBookAuthor = new Mock<BookAuthor>(2, author.Object, 2, book);
            book.SetAuthor(oldBookAuthor.Object);

            book.SetAuthor(newBookAuthor.Object);

            Assert.Contains(book.BookAuthors, ba => ba == newBookAuthor.Object);
            Assert.DoesNotContain(book.BookAuthors, ba => ba == oldBookAuthor.Object);
        }

        [Fact]
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

            Assert.True(book.BookAuthors.SequenceEqual(newBookAuthors));
            Assert.False(book.BookAuthors.Intersect(existedBookAuthors).Any());
        }

        [Fact]
        public void SetCardAndTerms_ReturnDateShouldBeLaterThanReceiveDate()
        {
            var book = new Book(title);

            book.SetCardAndTerms(1);

            Assert.True(book.ReturnDate.Value > book.ReceiveDate.Value);
        }
    }
}
