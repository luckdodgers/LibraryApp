using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Domain.Entities
{
    public class Author
    {
        public Author(string name)
        {
            Name = name;
        }

        private Author() { }

        public int Id { get; }
        public string Name { get; }

        private HashSet<BookAuthor> _bookAuthors = new HashSet<BookAuthor>();
        public IReadOnlyCollection<BookAuthor> BookAuthors => _bookAuthors.ToList();

        public void AddBook(BookAuthor bookAuthor) => _bookAuthors.Add(bookAuthor);
    }
}