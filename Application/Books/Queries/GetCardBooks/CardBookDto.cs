using System;
using System.Collections.Generic;

namespace LibraryApp.Application.Books.Queries.Common
{
    public class CardBookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IList<string> Authors { get; set; }
        public DateTime ReceiveDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
