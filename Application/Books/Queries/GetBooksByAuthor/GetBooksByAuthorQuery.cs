using LibraryApp.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Application.Books.Queries.GetBooksByAuthor
{
    public class GetBooksByAuthorQuery : IRequest<QueryResult<List<LibraryBookDto>>>
    {
        public string AuthorName { get; set; }
        public bool CurrentlyAvailableOnly { get; set; }
    }
}
