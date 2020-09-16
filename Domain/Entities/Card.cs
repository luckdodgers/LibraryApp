using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Domain.Entities
{
    public class Card
    {
        public int Id { get; }
        public List<Book> Books { get; } = new List<Book>();
    }
}
