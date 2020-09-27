using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Domain.Entities
{
    public class Card
    {
        public int Id { get; }
        public string UserName { get; }

        private HashSet<Book> _books = new HashSet<Book>();
        public IReadOnlyCollection<Book> Books => _books.ToList();

        public Card(string username)
        {
            UserName = username;
        }

        private Card() { }

        /// <returns>True if card doesn't contain book, otherwise false.</returns>
        public bool TryAddBook(Book book)
        {
            if (Books.Any(b => b.Id == book.Id))
                return false;

            book.SetCardAndTerms(Id);
            _books.Add(book);

            return true;
        }

        /// <returns>True if card contains book, otherwise false.</returns>
        public bool TryRemoveBook(Book book)
        {
            var bookToRemove = Books.FirstOrDefault(b => b.Id == book.Id);

            if (bookToRemove == null)
                return false;

            book.ResetCardAndTerms();
            _books.Remove(book);

            return true;
        }
    }
}
