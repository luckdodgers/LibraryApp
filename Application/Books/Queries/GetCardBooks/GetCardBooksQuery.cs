using LibraryApp.Application.Books.Queries.GetCardBooks;
using LibraryApp.Application.Common.Models;
using MediatR;
using System.Collections.Generic;

namespace LibraryApp.Application.Books.Queries
{
    public class GetCardBooksQuery : IRequest<QueryResult<List<CardBookDto>>>
    {
        public int CardId { get; }
        public string UserName { get; }

        public GetCardBooksQuery(int cardId, string userName)
        {
            CardId = cardId;
            UserName = userName;
        }
    }
}
