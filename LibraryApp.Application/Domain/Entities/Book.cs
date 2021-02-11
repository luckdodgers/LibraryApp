using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryApp.Domain.Entities
{
    public class Book
    {
        public Book(string title)
        {
            Title = title;
        }

        private Book() { }

        public virtual int Id { get; }
        public string Title { get; }
        private HashSet<BookAuthor> _bookAuthors = new HashSet<BookAuthor>();
        public IReadOnlyList<BookAuthor> BookAuthors => _bookAuthors.ToList();
        public DateTime? ReceiveDate { get; private set; }
        public DateTime? ReturnDate { get; private set; }
        public virtual int? CardId { get; private set; }

        public void SetAuthors(IEnumerable<BookAuthor> bookAuthors) => _bookAuthors = bookAuthors.ToHashSet();
        public void SetAuthor(BookAuthor bookAuthor)
        {
            if (_bookAuthors == null)
                _bookAuthors = new HashSet<BookAuthor>();

            _bookAuthors.Clear();
            _bookAuthors.Add(bookAuthor);
        }

        public void SetCardAndTerms(int cardId)
        {
            CardId = cardId;
            ReceiveDate = DateTime.Now;
            ReturnDate = DateTime.Now.AddDays(7);
        }

        public void ResetCardAndTerms()
        {
            CardId = null;
            ReceiveDate = null;
            ReturnDate = null;
        }
    }
}
