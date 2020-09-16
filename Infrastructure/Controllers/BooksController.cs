using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryApp.Application.Books.Queries;
using LibraryApp.Application.Books.Queries.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Infrastructure.Controllers
{
    public class BooksController : ApiController
    {
        [HttpGet("[controller]/cardId={cardId}")]
        public async Task<ActionResult<List<CardBookDto>>> GetCardBooks(int cardId)
        {
            return await Mediator.Send(new GetCardBooksQuery(cardId));
        }
    }
}
