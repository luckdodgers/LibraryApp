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
            var oldAuthor = new Mock<Author>("author_1");
            var newAuthor = new Mock<Author>("author_2");
            book.SetAuthor(oldAuthor.Object);

            book.SetAuthor(newAuthor.Object);

            book.Authors.Should().OnlyContain(ba => ba == newAuthor.Object);
        }

        [Test]
        public void SetSeveralAuthors_ShouldSetAndFlushPrevious()
        {
            var book = new Book(title);
            var oldAuthors = new List<Mock<Author>>()
            {
                new Mock<Author>("author_1"),
                new Mock<Author>("author_2"),
            };
            var newAuthors = new List<Mock<Author>>()
            {
                new Mock<Author>("author_3"),
                new Mock<Author>("author_4"),
            };
            book.SetAuthors(oldAuthors.Select(a => a.Object));

            book.SetAuthors(newAuthors.Select(a => a.Object));

            book.Authors.Should().BeEquivalentTo(newAuthors.Select(a => a.Object));
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
