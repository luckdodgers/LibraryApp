using System;
using System.Collections.Generic;

namespace LibraryApp.Application.Books.Queries.GetBooksByAuthor
{
    public class LibraryBookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IList<string> Authors { get; set; }
        public DateTime BecomeAvailableDate { get; set; }
    }
}