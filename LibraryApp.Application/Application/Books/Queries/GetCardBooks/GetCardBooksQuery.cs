﻿using LibraryApp.Application.Common.Models;
using MediatR;

namespace LibraryApp.Application.Books.Queries
{
    public class GetCardBooksQuery : IRequest<BaseResult>
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
