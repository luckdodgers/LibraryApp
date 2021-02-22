using LibraryApp.Application.Domain.Entities;
using System.Collections.Generic;

namespace LibraryApp.Domain.Entities
{
    public class Author : IDomainEntity
    {
        public Author(string name)
        {
            Name = name;
        }

        private Author() { }

        public int Id { get; }
        public string Name { get; }

        public ICollection<Book> Books { get; private set; } = new HashSet<Book>();

        public void AddBook(Book book) => Books.Add(book);
    }
}