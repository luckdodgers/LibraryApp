using LibraryApp.Application.Common.Models;
using MediatR;

namespace LibraryApp.Application.Books.Queries.GetBooksByAuthor
{
    public class GetBooksByAuthorQuery : IRequest<BaseResult>
    {
        public string AuthorName { get; set; }
        public bool CurrentlyAvailableOnly { get; set; }
    }
}
