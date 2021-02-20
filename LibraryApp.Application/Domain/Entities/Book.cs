using LibraryApp.Application.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryApp.Domain.Entities
{
    public class Book : IDomainEntity
    {
        public Book(string title)
        {
            Title = title;
        }

        private Book() { }

        public virtual int Id { get; }
        public string Title { get; }
        public ICollection<Author> Authors { get; private set; } = new HashSet<Author>();
        public DateTime? ReceiveDate { get; private set; }
        public DateTime? ReturnDate { get; private set; }
        public virtual int? CardId { get; private set; }

        public void SetAuthors(IEnumerable<Author> authors) => Authors = authors.ToHashSet();
        public void SetAuthor(Author author)
        {
            if (Authors == null)
                Authors = new HashSet<Author>();

            Authors.Clear();
            Authors.Add(author);
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
