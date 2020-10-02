using LibraryApp.Application.Books.Queries.GetCardBooks;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Application.Books.Queries
{
    public class GetCardBooksQuery : IRequest<List<CardBookDto>>
    {
        public int CardId { get; set; }
        public string UserName { get; set; }

        public GetCardBooksQuery(int cardId, string userName)
        {
            CardId = cardId;
            UserName = userName;
        }
    }
}
