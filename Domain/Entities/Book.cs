using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Domain.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public int Title { get; set; }
        public List<Author> Authors { get; set; } = new List<Author>();
        public DateTime ReceiveDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
