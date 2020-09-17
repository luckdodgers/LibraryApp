using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Domain.Entities
{
    public class Book
    {
        public Book(string title)
        {
            Title = title;
        }

        private Book() { }

        public int Id { get; }
        public string Title { get; }
        private HashSet<BookAuthor> _bookAuthors = new HashSet<BookAuthor>();
        public IReadOnlyList<BookAuthor> BookAuthors => _bookAuthors.ToList();
        public DateTime? ReceiveDate { get; private set; }
        public DateTime? ReturnDate { get; private set; }

        public void SetAuthors(IEnumerable<BookAuthor> bookAuthors) => _bookAuthors = bookAuthors.ToHashSet();
        public void SetAuthors(BookAuthor bookAuthor)
        {
            if (_bookAuthors == null)
                _bookAuthors = new HashSet<BookAuthor>();

            _bookAuthors.Add(bookAuthor);
        }

        public void SetBorrowTerms()
        {
            ReceiveDate = DateTime.Now;
            ReturnDate = DateTime.Now.AddDays(7);
        }

        public void ResetBorrowTerms()
        {
            ReceiveDate = null;
            ReturnDate = null;
        }
    }
}
